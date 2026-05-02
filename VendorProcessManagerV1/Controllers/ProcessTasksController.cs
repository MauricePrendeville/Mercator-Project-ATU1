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
using VendorProcessManagerV1.Services;
using VendorProcessManagerV1.ViewModels;

namespace VendorProcessManagerV1.Controllers
{
    public class ProcessTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProcessTaskService _processTaskService;

        public ProcessTasksController(ApplicationDbContext context,
                                        UserManager<AppUser> userManager, 
                                        IProcessTaskService processTaskService)
        {
            _context = context;
            _userManager = userManager;
            _processTaskService = processTaskService;
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
        public async Task<IActionResult> Edit(Guid id, EditProcessTaskViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                vm.OwnerOptions = await BuildUserOptions(vm.OwnerId);
                vm.ApproverOptions = await BuildUserOptions(vm.ApproverId);
                vm.StatusOptions = BuildStatusOptions(vm.ProcessTaskStatus);
                vm.ApproveStatusOptions = BuildApproveStatusOptions(vm.ApproveStatus);
                return View(vm);
            }

            var task = await _context.ProcessTasks.FindAsync(id);

            if (task == null)
                return NotFound();

            task.OwnerId = vm.OwnerId;
            task.TaskNotes = vm.TaskNotes;
            task.StartedDate = vm.StartedDate;
            task.ProcessTaskStatus = vm.ProcessTaskStatus;
            task.IsCompleted = vm.IsCompleted;
            task.ApproverId = vm.ApproverId;
            task.ApproveStatus = vm.ApproveStatus;
            task.ApproveDate = vm.ApproveDate;
            task.UpdatedDate = DateTime.Now;

            if (vm.IsCompleted && task.CompletedDate == null)
                task.CompletedDate = DateTime.Now;

            if (vm.ProcessTaskStatus == ProcessTaskStatus.InProgress
                && task.StartedDate == null)
                task.StartedDate = DateTime.Now; 

            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                var fullError = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError(string.Empty, fullError);

                vm.OwnerOptions = await BuildUserOptions(vm.OwnerId);
                vm.ApproverOptions = await BuildUserOptions(vm.ApproverId);
                vm.StatusOptions = BuildStatusOptions(vm.ProcessTaskStatus);
                vm.ApproveStatusOptions = BuildApproveStatusOptions(vm.ApproveStatus);
                return View(vm); 
            }
            
            return RedirectToAction("Details", "ProcessInstances", 
                new { id = vm.ProcessInstanceId});
                       
        }

        //GET: ProcessTasks/Complete/id
        //Shows availabel transtiotns to the user
        public async Task<IActionResult> Complete(Guid id)
        {
            var task = await _context.ProcessTasks
                .Include(t => t.ProcessInstance)
                .FirstOrDefaultAsync(task => task.Id == id);

            if (task == null)
                return NotFound(); 

            if(task.ApprovalRequired && 
                task.ApproveStatus != ApproveStatus.Approved)
            {
                TempData["Error"] = "This task requires approval before it can be completed.";
                return RedirectToAction("Details", "ProcessInstances",
                    new { id = task.ProcessInstanceId }); 
            }

            var transitions = await _processTaskService
                .GetAvailableTransitionsAsync(id);

            var vm = new CompleteTaskViewModel
            {
                TaskId = id,
                TaskTitle = task.Title,
                ProcessInstanceId = task.ProcessInstanceId,
                Transitions = transitions,
                HasTransitions = transitions.Any(),
                DefaultTransitionId = transitions
                    .FirstOrDefault(t => t.IsDefault)?.Id
            };

            return View(vm); 
        }

        //POST: ProcessTasks/Complete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(CompleteTaskViewModel vm)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var result = await _processTaskService.CompleteTaskAsync(
                vm.TaskId,
                vm.SelectedTransitionId,
                currentUser.Id);

            if (!result.Succeeded)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Details", "ProcessInstances",
                    new { id = vm.ProcessInstanceId }); 
            }

            if (result.IsProcessComplete)
            {
                TempData["Success"] = "Process completed successfully.";
                return RedirectToAction("Details", "ProcessInstances",
                    new { id = vm.ProcessInstanceId });
            }

            TempData["Success"] = "Task completed.";
            return RedirectToAction("Details", "ProcessInstances",
                new { id = vm.ProcessInstanceId });
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
