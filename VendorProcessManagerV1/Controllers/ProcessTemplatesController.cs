using MermaidDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.Services;
using VendorProcessManagerV1.ViewModels;

namespace VendorProcessManagerV1.Controllers
{
    public class ProcessTemplatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProcessInstanceService _processInstanceService;

        public ProcessTemplatesController(ApplicationDbContext context,
                UserManager<AppUser> userManager,
                IProcessInstanceService processInstanceService)
        {
            _context = context;
            _userManager = userManager;
            _processInstanceService = processInstanceService;
        }

        // GET: ProcessTemplates
        public async Task<IActionResult> Index(ProcessTemplateIndexViewModel vm)
        {
            vm.SortOrder ??= "templatename_asc";

            vm.TemplateNameSort = vm.SortOrder == "templatename_asc" ? "templatename_desc" : 
                "templatename_asc";
            vm.DescriptionSort = vm.SortOrder == "description_asc" ? "description_desc" : "description_asc";
            vm.CategorySort = vm.SortOrder == "category_asc" ? "category_desc" : "category_asc";
            vm.CreatorSort = vm.SortOrder == "creator_asc" ? "creator_desc" : "creator_asc";
            vm.ActiveSort = vm.SortOrder == "active_asc" ? "active_desc" : "active_asc";
            

            var templateNames = await _context.ProcessTemplates
                .Where(t => !string.IsNullOrEmpty(t.Name))
                .Select(t => t.Name)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            vm.TemplateList = new SelectList(templateNames, vm.SelectedTemplateName);

            var categories = await _context.ProcessTemplates
                .Where(t => !string.IsNullOrEmpty(t.Category))
                .Select(t => t.Category)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            vm.CategoryList = new SelectList(categories, vm.CategoryFilter);

            vm.ActiveList = new SelectList(new[]
            {
                new {Value = "true", Text = "Active" },
                new {Value = "false", Text ="Inactive" }
                }, "Value", "Text", vm.ActiveFilter);

                        
            var templates = _context.ProcessTemplates
                .Include(t => t.Creator)
                .AsQueryable();


            if (!string.IsNullOrEmpty(vm.SelectedTemplateName))
            {
                templates = templates
                    .Where(t => t.Name == vm.SelectedTemplateName);
            }

            if (!string.IsNullOrEmpty(vm.CategoryFilter))
            {
                templates = templates
                    .Where(t => t.Category == vm.CategoryFilter);
            }

            if (!string.IsNullOrEmpty(vm.ActiveFilter))
            {
                var isActive = vm.ActiveFilter == "true";
                templates = templates
                    .Where(t => t.IsActive == isActive);
            }

            templates = vm.SortOrder switch
            {
                "templatename_asc" => templates.OrderBy(t => t.Name),
                "templatename_desc" => templates.OrderByDescending(t => t.Name),
                "description_asc" => templates.OrderBy(t => t.Description),
                "description_desc" => templates.OrderByDescending(t => t.Description),
                "category_asc" => templates.OrderBy(t => t.Category),
                "category_desc" => templates.OrderByDescending(t => t.Category),
                "creator_asc" => templates.OrderBy(p => p.Creator.LastName),
                "creator_desc" => templates.OrderByDescending(p => p.Creator.LastName),
                "active_asc" => templates.OrderBy(t => t.IsActive),
                "active_desc" => templates.OrderByDescending(t => t.IsActive),
                _ => templates.OrderBy(t => t.Name)
            };

            vm.Templates = await templates.ToListAsync();

            return View(vm);            
        }

        // GET: ProcessTemplates/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplates
                .Include(t => t.Creator)
                .Include(t => t.Tasks)
                    .ThenInclude(task => task.Transitions)
                        .ThenInclude(tr => tr.ToProcessTemplateTask)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (processTemplate == null)
            {
                return NotFound();
            }

            //adding mermaid flowchart capability. Uses MermaidDotNet
            //var diagram = BuildTemplateDiagram(processTemplate);
            //ViewBag.MermaidDiagram = diagram;
            ViewBag.MermaidDiagram = BuildTemplateDiagram(processTemplate);
            ViewBag.CurrentUserTeam = (await _userManager.GetUserAsync(User))?.Team;

            return View(processTemplate);
        }

       
        //GET ProcessTemplates/Create with dropdown list
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var vm = new CreateProcessTemplateViewModel
            {
                CreateDate = DateTime.Now, //check the default timezone later
                IsActive = true,
                CreatorName = currentUser.FirstName + " " + currentUser.LastName

                /*
                CreatorOptions = new SelectList(
                    await _context.Users
                    .OrderBy(u => u.LastName)
                    .Select(u => new {
                        u.Id, FullName = u.FirstName + " " + u.LastName
                    })
                    .ToListAsync(),
                    "Id", //CreatorId 
                    "FullName" //dropdown list
                   )
                */
            };
            return View(vm);
        }

        // POST: ProcessTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateProcessTemplateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                //await PopulateCreatorDropdown(vm);
                return View(vm);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var processTemplate = new ProcessTemplate
            {
                Id = Guid.NewGuid(),
                Name = vm.Name,
                Description = vm.Description,
                Category = vm.Category,
                CreateDate = DateTime.Now,
                IsActive = true,
                Version = vm.Version,

                CreatorId = currentUser.Id
            };

            _context.Add(processTemplate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: ProcessTemplates/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplates.FindAsync(id);
            if (processTemplate == null)
            {
                return NotFound();
            }
            return View(processTemplate);
        }

        // POST: ProcessTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Category,Creator,CreateDate,IsActive,Version")] ProcessTemplate processTemplate)
        {
            if (id != processTemplate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessTemplateExists(processTemplate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(processTemplate);
        }

        // GET: ProcessTemplates/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTemplate == null)
            {
                return NotFound();
            }

            return View(processTemplate);
        }

        // POST: ProcessTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var processTemplate = await _context.ProcessTemplates.FindAsync(id);
            if (processTemplate != null)
            {
                _context.ProcessTemplates.Remove(processTemplate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTemplateExists(Guid id)
        {
            return _context.ProcessTemplates.Any(e => e.Id == id);
        }

        private string BuildTemplateDiagram(ProcessTemplate template)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("flowchart TD");
            sb.AppendLine();

            var sortedTasks = template.Tasks.OrderBy(t => t.SortOrder).ToList();

            var firstTask = sortedTasks.FirstOrDefault();
            if (firstTask != null)
            {
                var firstRef = "T" + firstTask.Id.ToString("N").Substring(0, 8);
                sb.AppendLine($"    START([Start]) --> {firstRef}");
            }

            foreach (var task in sortedTasks)
            {
                var taskRef = "T" + task.Id.ToString("N").Substring(0, 8);
                var safeTitle = task.Title
                    .Replace("\"", "'")
                    .Replace("\n", " ")
                    .Trim();
                
                //wrapped title added to make the node text more readable
                var wrappedTitle = WrapText(safeTitle, 22); 
                
                sb.AppendLine($"    {taskRef}[\"{task.SortOrder}.{wrappedTitle}\"]");
                
                    if (task.Transitions != null && task.Transitions.Any())
                    {
                        foreach(var tr in task.Transitions.OrderBy(t => t.SortOrder))
                        {
                            var safeLabel = tr.DisplayLabel
                                .Replace("\"", "'")
                                .Trim();

                            if (tr.ToProcessTemplateTaskId.HasValue)
                            {
                                var targetRef = "T" + tr.ToProcessTemplateTaskId.Value
                                    .ToString("N").Substring(0, 8);
                                sb.AppendLine(
                                    $"    {taskRef} -->|\"{safeLabel}\"| {targetRef}");
                            }
                            else
                            {
                                sb.AppendLine(
                                    $"    {taskRef} -->|\"{safeLabel}\"| " +
                                    $"END([End of process])");
                            }
                        }
                    }
                    else
                    { 
                        var next = sortedTasks
                            .SkipWhile(t => t.Id != task.Id)
                            .Skip(1)
                            .FirstOrDefault(); 

                        if (next != null)
                        {
                            var nextRef = "T" + next.Id.ToString("N").Substring(0, 8);
                            sb.AppendLine($"    {taskRef} --> {nextRef}");
                        }
                        else
                        {
                            sb.AppendLine($"    {taskRef} --> COMPLETE([Complete])");
                        }

                    }

            }
            return sb.ToString();
        }

        /// <summary>
        /// Helper method to wrap the text in the 
        /// Nodes in the Mermaid flowcharts
        /// </summary>
        /// <param name="text">Node title text</param>
        /// <param name="maxWidth">Width of text before wrapping occurs</param>
        /// <returns>The node title text with line breaks every maxWidth characters</returns>
        private string WrapText(string text, int maxWidth = 23)
        {
            if (text.Length <= maxWidth) 
                return text;

            var words = text.Split(' ');
            var lines = new List<string>();
            var current = new System.Text.StringBuilder();

            foreach(var word in words)
            {
                if (current.Length + word.Length + 1> maxWidth 
                    && current.Length >0)
                {
                    lines.Add(current.ToString().Trim());
                    current.Clear();
                }
                current.Append(word + " ");
            }
            if (current.Length > 0)
                lines.Add(current.ToString().Trim());

            return string.Join("<br/>", lines);
        }
        //private async Task PopulateCreatorDropdown(CreateProcessTemplateViewModel vm)
        //{
        //    vm.CreatorOptions = new SelectList(
        //        await _context.Users
        //        .OrderBy(u => u.LastName)
        //        .Select(u => new
        //        {
        //            u.Id,
        //            FullName = u.LastName + " " + u.LastName
        //        })
        //        .ToListAsync(),
        //        "Id",
        //        "FullName",
        //        vm.Id

        //    );
        //}

       /* //GET: ProcessTemplates/StartInstance/templateId
        public async Task<IActionResult> StartInstance(Guid id)
        {
            var template = await _context.ProcessTemplates
                .Include(t => t.Tasks)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
                return NotFound(); 

            if (!template.Tasks.Any())
            {
                TempData["Error"] = "Cannot start an instance this template has no tasks.";
                return RedirectToAction("Details", new { id });
            }

            var vm = new StartProcessInstanceViewModel
            {
                ProcessTemplateId = template.Id,
                TemplateName = template.Name,
                TaskCount = template.Tasks.Count,
                SuggestedName = template.Name + "-" +
                                DateTime.Now.ToString("dd MM yyyy")
            };

            return View(vm);
        }

        //POST ProcessTemplates/StartInstance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartInstance(StartProcessInstanceViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            try
            {
                var instance = await _processInstanceService.StartInstanceAsync(
                    vm.ProcessTemplateId,
                    currentUser.Id);

                TempData["Success"] = "Process instance started successfully.";

                //redirect
                return RedirectToAction("Details", "ProcessInstances",
                    new { id = instance.Id });
            }

            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }*/
    }
}
