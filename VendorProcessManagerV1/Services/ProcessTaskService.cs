using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;


namespace VendorProcessManagerV1.Services
{
    public class ProcessTaskService : IProcessTaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProcessTaskService(ApplicationDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Checks to see if the Task can be started be checking preceeding task 
        /// is completed or skipped.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<bool> CanStartTaskAsync(Guid taskId)
        {
            var task = await _context.ProcessTasks
                .Include(t => t.ProcessInstance)
                    .ThenInclude(i => i.Tasks)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) return false;

            if (task.SortOrder == 1) return true;

            var predecessors = task.ProcessInstance.Tasks
                .Where(t => t.SortOrder < task.SortOrder &&
                            t.IsActive)
                .ToList();

            if (!predecessors.Any()) return true;

            return predecessors.All(t =>
                t.ProcessTaskStatus == ProcessTaskStatus.Completed ||
                //t.ProcessTaskStatus == ProcessTaskStatus.Approved ||//
                t.ProcessTaskStatus == ProcessTaskStatus.Skipped); 
        }

        /// <summary>
        /// Gets the different options for the next step in the flowchart. 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<List<ProcessTemplateTransition>> GetAvailableTransitionsAsync(
            Guid taskId)
        {
            var task = await _context.ProcessTasks
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
                return new List<ProcessTemplateTransition>();

            return await _context.ProcessTemplateTransitions
                .Include(t => t.ToProcessTemplateTask)
                .Where(t => t.FromProcessTemplateTaskId == 
                            task.ProcessTemplateTaskId)
                .OrderBy(t => t.SortOrder)
                .ToListAsync();
        }
        /// <summary>
        /// Sets Task to Completed. Selects next task in process by checking for transition. 
        /// Sets next selected task to In Progress. 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="selectedTransitionId"></param>
        /// <param name="completedById"></param>
        /// <returns></returns>
        public async Task<CompleteTaskResult> CompleteTaskAsync(
            Guid taskId, 
            Guid? selectedTransitionId, 
            string completedById)
        {
            var task = await _context.ProcessTasks
                .Include(t => t.ProcessInstance)
                    .ThenInclude(i => i.Tasks)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
                return new CompleteTaskResult
                {
                    Succeeded = false,
                    ErrorMessage = "Task not found."
                };

            if(task.ApprovalRequired && task.ApproveStatus != ApproveStatus.Approved)
            {
                return new CompleteTaskResult
                {
                    Succeeded = false,
                    ErrorMessage = task.ApproveStatus == ApproveStatus.Rejected ?
                        "This task has been rejected and cannot be completed." :
                        "This task requires approval before it can be comlpeted"

                };
            }


            task.ProcessTaskStatus = ProcessTaskStatus.Completed;
            task.IsCompleted = true;
            task.CompletedDate = DateTime.Now;
            task.SelectedTransitionId = selectedTransitionId;

            var transitions = await GetAvailableTransitionsAsync(taskId); 

            if(transitions.Any())
            {
                ProcessTemplateTransition? selectedTransition = null;

                if (selectedTransitionId.HasValue)
                {
                    selectedTransition = transitions
                        .FirstOrDefault(t => t.Id == selectedTransitionId); 
                }
                else
                {
                    selectedTransition = transitions
                        .FirstOrDefault(t => t.IsDefault) 
                        ?? transitions.First(); 
                }

                if(selectedTransition != null)
                {
                    if (selectedTransition.ToProcessTemplateTaskId.HasValue)
                    {
                        var nextTask = task.ProcessInstance.Tasks
                            .FirstOrDefault(t =>
                                t.ProcessTemplateTaskId ==
                                selectedTransition.ToProcessTemplateTaskId); 

                        if(nextTask != null)
                        {
                            nextTask.IsActive = true;
                            nextTask.ProcessTaskStatus = ProcessTaskStatus.InProgress;
                            nextTask.StartedDate = DateTime.Now;

                            await DeactivateUnselectedBranchesAsync(
                                task, selectedTransition, transitions);

                            await _context.SaveChangesAsync();

                            return new CompleteTaskResult
                            {
                                Succeeded = true,
                                NextTaskId = nextTask.Id
                            };
                        }
                    }
                    else
                    {
                        await CompleteInstanceAsync(task.ProcessInstance);
                        await _context.SaveChangesAsync();

                        return new CompleteTaskResult
                        {
                            Succeeded = true,
                            IsProcessComplete = true
                        }; 
                    }
                }
            }
            else
            {
                var nextTask = task.ProcessInstance.Tasks
                    .Where(t => t.SortOrder == task.SortOrder + 1)
                    .FirstOrDefault(); 

                if(nextTask != null)
                {
                    nextTask.IsActive = true;
                    nextTask.ProcessTaskStatus = ProcessTaskStatus.InProgress;
                    nextTask.StartedDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    return new CompleteTaskResult
                    {
                        Succeeded = true,
                        NextTaskId = nextTask.Id
                    }; 
                }
                else
                {
                    await CompleteInstanceAsync(task.ProcessInstance);
                    await _context.SaveChangesAsync();

                    return new CompleteTaskResult
                    {
                        Succeeded = true,
                        IsProcessComplete = true
                    }; 
                }
            }
            await _context.SaveChangesAsync();
            return new CompleteTaskResult { Succeeded = true }; 
        }
        /// <summary>
        /// Checks the User's Team to see if they can approve the task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CanApproveTaskAsync(Guid taskId, string userId)
        {
            var task = await _context.ProcessTasks
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) return false;

            //task must require approval to be checked
            if (!task.ApprovalRequired) return false;

            if (task.ProcessTaskStatus != ProcessTaskStatus.InProgress &&
                task.ProcessTaskStatus != ProcessTaskStatus.Completed)
                return false; 

            //task already approved
            if(task.ApproveStatus == ApproveStatus.Approved || 
                task.ApproveStatus == ApproveStatus.Rejected) 
                return false;

            //check user and team
            var user = await _userManager.FindByIdAsync(userId); 
            if (user == null) return false;

            //is approver team set in Task
            if (string.IsNullOrEmpty(task.ApproverTeam)) 
                return true;

            //does user team match required team
            return string.Equals(
                user.Team,
                task.ApproverTeam,
                StringComparison.OrdinalIgnoreCase); 
        }

        /// <summary>
        /// checks to see if user has permission to Approve task. User must 
        /// be member of Approver Team before setting task to approved. 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <param name="decision"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        public async Task<ApproveTaskResult> ApproveTaskAsync(
            Guid taskId, 
            string userId, 
            ApproveStatus decision, 
            string? notes)
        {
            var canApprove = await CanApproveTaskAsync(taskId, userId);
            if (!canApprove)
                return new ApproveTaskResult
                {
                    Succeeded = false,
                    ErrorMessage = "You do not have permission to approve this task. " +
                                    "Approval is restricted to the designated team."
                }; 

            var task = await _context.ProcessTasks 
                .Include(t => t.ProcessInstance)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
                return new ApproveTaskResult
                {
                    Succeeded = false,
                    ErrorMessage = "Task not found"
                };

            task.ApproveStatus = decision;
            task.ApproverId = userId;
            task.ApproveDate = DateTime.Now;

            if (notes != null)
                task.TaskNotes = notes; 
            if( decision == ApproveStatus.Rejected) 
            {
                task.ProcessTaskStatus = ProcessTaskStatus.Rejected;
            }

            _context.Update(task); 
            await _context.SaveChangesAsync();

            return new ApproveTaskResult { Succeeded = true }; 
        }

        /// <summary>
        /// Sets unselected task option in process to Skipped
        /// </summary>
        /// <param name="completedTask"></param>
        /// <param name="selectedTransition"></param>
        /// <param name="allTransitions"></param>
        /// <returns></returns>
        private async Task DeactivateUnselectedBranchesAsync(
            ProcessTask completedTask, 
            ProcessTemplateTransition selectedTransition, 
            List<ProcessTemplateTransition> allTransitions)
        {
            var unselectedTransitions = allTransitions
                .Where(t => t.Id != selectedTransition.Id &&
                            t.ToProcessTemplateTaskId.HasValue)
                .ToList(); 

            foreach (var transition in unselectedTransitions)
            {
                var branchTask = completedTask.ProcessInstance.Tasks
                    .FirstOrDefault(t =>
                    t.ProcessTemplateTaskId ==
                    transition.ToProcessTemplateTaskId); 

                if(branchTask != null && 
                    branchTask.ProcessTaskStatus == ProcessTaskStatus.NotStarted)
                {
                    branchTask.IsActive = false;
                    branchTask.ProcessTaskStatus = ProcessTaskStatus.Skipped; 
                }
            }
        }

        /// <summary>
        /// Sets process instance status to Completed and actual end date to now. 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        private async Task CompleteInstanceAsync(ProcessInstance instance)
        {
            instance.Status = ProcessInstanceStatus.Completed;
            instance.ActualEndDate = DateTime.Now; 
        }
    }
}
