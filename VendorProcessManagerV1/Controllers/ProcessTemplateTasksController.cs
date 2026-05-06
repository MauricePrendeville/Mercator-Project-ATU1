using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.ViewModels;

namespace VendorProcessManagerV1.Controllers
{
    public class ProcessTemplateTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProcessTemplateTasksController(ApplicationDbContext context,
                                               UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ProcessTemplateTasks
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProcessTemplatesTasks.ToListAsync());
        }

        // GET: ProcessTemplateTasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTask = await _context.ProcessTemplatesTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTemplateTask == null)
            {
                return NotFound();
            }

            return View(processTemplateTask);
        }

        // GET: ProcessTemplateTasks/Create
        public async Task<IActionResult> Create(Guid templateId)
        {
            var template = await _context.ProcessTemplates.FindAsync(templateId);

            if (template == null)
                return NotFound();

            var vm = new CreateProcessTemplateTaskViewModel
            {
                ProcessTemplateId = templateId,
                TemplateName = template.Name,
                ApprovalRequired = false,
                SortOrder = 0,
                ApproverTeamOptions = await BuildTeamOptions()

            };

            return View(vm);
        }

        // POST: ProcessTemplateTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProcessTemplateTaskViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.ApproverTeamOptions = await BuildTeamOptions(vm.ApproverTeam);
                return View(vm);
            }

            var task = new ProcessTemplateTask
            {
                Id = Guid.NewGuid(),
                Title = vm.Title,
                Description = vm.Description,
                ProcessTemplateId = vm.ProcessTemplateId,
                //ApproverId = vm.ApproverId, 
                ApproverTeam = vm.ApproverTeam,
                ApprovalRequired = vm.ApprovalRequired,
                SortOrder = vm.SortOrder,
                DefaultOwnerRole = vm.DefaultOwnerRole
            };

            _context.ProcessTemplatesTasks.Add(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "ProcessTemplates",
                new { id = vm.ProcessTemplateId });

        }

        // GET: ProcessTemplateTasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTask = await _context.ProcessTemplatesTasks
                .FirstOrDefaultAsync(t => t.Id == id);

            if (processTemplateTask == null)
            {
                return NotFound();
            }

            var vm = new EditProcessTemplateTaskViewModel
            {
                Id = processTemplateTask.Id,
                ProcessTemplateId = processTemplateTask.ProcessTemplateId,
                Title = processTemplateTask.Title,
                Description = processTemplateTask.Description,
                ApproverTeam = processTemplateTask.ApproverTeam,
                ApprovalRequired = processTemplateTask.ApprovalRequired,
                SortOrder = processTemplateTask.SortOrder,
                DefaultOwnerRole = processTemplateTask.DefaultOwnerRole,
                ApproverTeamOptions = await BuildTeamOptions(processTemplateTask.ApproverTeam)
            };

            return View(vm);
        }

        // POST: ProcessTemplateTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id, EditProcessTemplateTaskViewModel vm)
                           
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                vm.ApproverTeamOptions = await BuildTeamOptions(vm.ApproverTeam);
                return View(vm);
            }
             
            var task = await _context.ProcessTemplatesTasks.FindAsync(id);
            if (task == null)
                return NotFound();

            task.Title = vm.Title;
            task.Description = vm.Description;
            task.ApproverTeam  = vm.ApproverTeam;
            task.ApprovalRequired = vm.ApprovalRequired;
            task.SortOrder = vm.SortOrder;
            task.DefaultOwnerRole = vm.DefaultOwnerRole;

            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ProcessTemplatesTasks.Any(t => t.Id == vm.Id))
                    return NotFound();
                throw;
            }
            
            return RedirectToAction("Details", "ProcessTemplates",
                new { id = vm.ProcessTemplateId });
}


        // GET: ProcessTemplateTasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTask = await _context.ProcessTemplatesTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTemplateTask == null)
            {
                return NotFound();
            }

            return View(processTemplateTask);
        }

        // POST: ProcessTemplateTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var processTemplateTask = await _context.ProcessTemplatesTasks.FindAsync(id);
            if (processTemplateTask != null)
            {
                _context.ProcessTemplatesTasks.Remove(processTemplateTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTemplateTaskExists(Guid id)
        {
            return _context.ProcessTemplatesTasks.Any(e => e.Id == id);
        }

        private async Task<SelectList> BuildApproverOptions(string? selectId = null)
        {
            var users = await _userManager.Users
                .OrderBy(u => u.LastName)
                .Select(u => new
                {
                    u.Id,
                    FullName = u.FirstName + " " + u.LastName
                })
                .ToListAsync();
            return new SelectList(users, "Id", "FullName", selectId);    
        }

        private async Task<SelectList>BuildTeamOptions(string? selectedTeam = null) 
        {
            var teams = await _userManager.Users
                .Where(u => u.Team != null && u.Team != "")
                .Select(u => u.Team)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            return new SelectList(
                teams.Select(t => new {Value = t, Text = t}),
                "Value", 
                "Text", 
                selectedTeam);
        }
    }
}
