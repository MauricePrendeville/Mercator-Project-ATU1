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
        public async Task <IActionResult> Create(Guid templateId)
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
                ApproverOptions = await BuildApproverOptions()

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
                vm.ApproverOptions = await BuildApproverOptions(vm.ApproverId);
                return View(vm);
            }

            var task = new ProcessTemplateTask
            {
                Id = Guid.NewGuid(),
                Title = vm.Title,
                Description = vm.Description,
                ProcessTemplateId = vm.ProcessTemplateId,
                ApproverId = vm.ApproverId, 
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

            var processTemplateTask = await _context.ProcessTemplatesTasks.FindAsync(id);
            if (processTemplateTask == null)
            {
                return NotFound();
            }
            return View(processTemplateTask);
        }

        // POST: ProcessTemplateTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TemplateID,TaskId,Title,Description,ApproverId,ApproverTeam,ApprovalRequired,SortOrder,DefaultOwnerRole")] ProcessTemplateTask processTemplateTask)
        {
            if (id != processTemplateTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processTemplateTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessTemplateTaskExists(processTemplateTask.Id))
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
            return View(processTemplateTask);
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
    }
}
