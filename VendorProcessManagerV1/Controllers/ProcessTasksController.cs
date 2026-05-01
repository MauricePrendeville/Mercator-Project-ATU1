using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.ViewModels;

namespace VendorProcessManagerV1.Controllers
{
    public class ProcessTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProcessTasksController(ApplicationDbContext context,
                                        UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ProcessTasks
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProcessTasks.ToListAsync());
        }

        // GET: ProcessTasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTask = await _context.ProcessTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTask == null)
            {
                return NotFound();
            }

            return View(processTask);
        }

        // GET: ProcessTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Task_Id,Title,Description,CreatedDate,UpdatedDate,CompletedDate,Creator,Owner,ApproverTeam,Approver,ApproveDate,ApproveStatus,TaskStatus,IsCompleted,RequiresApproval")] ProcessTask processTask)
        {
            if (ModelState.IsValid)
            {
                processTask.Id = Guid.NewGuid();
                _context.Add(processTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processTask);
        }

        // GET: ProcessTasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var task = await _context.ProcessTasks
                .Include(t => t.ProcessInstance)
                .FirstOrDefaultAsync(t => t.Id == id); 
            
            if (id == null)
            {
                return NotFound();
            }

            var vm = new EditProcessTaskViewModel
            {
                Id = task.Id, 
                Title = task.Title,
                Description = task.Description,
                SortOrder = task.SortOrder,
                ApprovalRequired = task.ApprovalRequired,
                ApproverTeam = task.ApproverTeam,
                ProcessInstanceId = task.ProcessInstanceId,
                OwnerId = task.OwnerId,
                TaskNotes = task.TaskNotes,
                StartedDate = task.StartedDate,
                ProcessTaskStatus   = task.ProcessTaskStatus,
                IsCompleted = task.IsCompleted,
                CompletedDate = task.CompletedDate,
                ApproverId = task.ApproverId,
                ApproveStatus = task.ApproveStatus,
                ApproveDate = task.ApproveDate,
                OwnerOptions = await BuildUserOptions(task.OwnerId), 
                ApproverOptions = await BuildUserOptions(task.ApproverId), 
                StatusOptions = BuildStatusOptions(task.ProcessTaskStatus), 
                ApproveStatusOptions = BuildApproveStatusOptions(task.ApproveStatus)
            };
            return View(vm);
        }

        // POST: ProcessTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Task_Id,Title,Description,CreatedDate,UpdatedDate,CompletedDate,Creator,Owner,ApproverTeam,Approver,ApproveDate,ApproveStatus,TaskStatus,IsCompleted,RequiresApproval")] ProcessTask processTask)
        {
            if (id != processTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessTaskExists(processTask.Id))
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
            return View(processTask);
        }

        // GET: ProcessTasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTask = await _context.ProcessTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTask == null)
            {
                return NotFound();
            }

            return View(processTask);
        }

        // POST: ProcessTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var processTask = await _context.ProcessTasks.FindAsync(id);
            if (processTask != null)
            {
                _context.ProcessTasks.Remove(processTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTaskExists(Guid id)
        {
            return _context.ProcessTasks.Any(e => e.Id == id);
        }

        private async Task<SelectList>BuildUserOptions(string? selectedId = null)
        {
            var users = await _userManager.Users
                .OrderBy(u => u.LastName)
                .Select(u => new { u.Id, FullName = u.FirstName + " " + u.LastName })
                .ToListAsync();
            return new SelectList(users, "Id", "FullName", selectedId); 
        }

        private SelectList BuildStatusOptions(ProcessTaskStatus? selected = null)
        {
            var statuses = Enum.GetValues(typeof(ProcessTaskStatus))
                .Cast<ProcessTaskStatus>()
                .Select(s => new {Value = s, Text = s.ToString()})
                .ToList();
            return new SelectList(statuses, "Value", "Text", selected);
        }

        private SelectList BuildApproveStatusOptions(ApproveStatus? selected = null)
        {
            var statuses = Enum.GetValues(typeof(ApproveStatus))
                .Cast<ApproveStatus>()
                .Select(s => new { Value = s, Text = s.ToString() })
                .ToList();
            return new SelectList(statuses, "Value", "Text", selected);
        }
    }
}
