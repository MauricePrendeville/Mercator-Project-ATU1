using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.Services
{
    /// <summary>
    /// A service class that can start a new process instance based on a template.
    /// </summary>
    public class ProcessInstanceService : IProcessInstanceService
    {
        private readonly ApplicationDbContext _context; 

        /// <summary>
        /// Constructor class for the service. 
        /// </summary>
        /// <param name="context"> Used to access the database.</param>
        public ProcessInstanceService(ApplicationDbContext context)
        {
            _context = context; 
        }

        /// <summary>
        /// Method to start a new process instance. 
        /// </summary>
        /// <param name="templateId"> The identifier for the template.</param>
        /// <param name="instanceName"> The name of the process instance</param>
        /// <param name="vendorCandidateId"> The identifier for the vendor candidate</param>
        /// <param name="initiatedById"> User that starts the instance.</param>
        /// <returns>A new process instance if successful. Otherwise an exception message.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ProcessInstance> StartInstanceAsync(
            Guid templateId, 
            string instanceName,
            Guid vendorCandidateId,
            string initiatedById)
        {
            var template = await _context.ProcessTemplates
                .Include(t => t.Tasks)
                    .ThenInclude(task => task.Transitions)
                .FirstOrDefaultAsync(t => t.Id == templateId);

            if (template == null)
                throw new ArgumentException("Template not found");

            if (!template.Tasks.Any())
                throw new InvalidOperationException(
                    "Cannot start an instance from a template with no tasks");

            var instance = new ProcessInstance
            {
                Id = Guid.NewGuid(), 
                ProcessTemplateId = templateId, 
                InstanceName = instanceName, 
                InitiatedById = initiatedById, 
                VendorCandidateId = vendorCandidateId,
                CreatedDate = DateTime.Now, 
                Status = ProcessInstanceStatus.InProgress
            };

            var sortedTasks = template.Tasks.OrderBy(t => t.SortOrder).ToList();

            for (int i = 0; i < sortedTasks.Count; i++)
            {
                var templateTask = sortedTasks[i];

                var instanceTask = new ProcessTask
                {
                    Id = Guid.NewGuid(),
                    ProcessInstanceId = instance.Id,
                    ProcessTemplateTaskId = templateTask.Id,

                    Title = templateTask.Title,
                    Description = templateTask.Description,
                    ApproverTeam = templateTask.ApproverTeam,
                    ApprovalRequired = templateTask.ApprovalRequired,
                    SortOrder = templateTask.SortOrder,
                    ProcessTaskStatus = i == 0? ProcessTaskStatus.InProgress :
                                                ProcessTaskStatus.NotStarted,
                    IsActive = i == 0,
                    StartedDate = null,
                    CompletedDate = null, 
                    CreatorId = initiatedById, 
                    OwnerId = initiatedById, 
                    ApproveStatus = templateTask.ApprovalRequired
                                    ? ApproveStatus.Pending 
                                    : ApproveStatus.NotRequired
                };

                instance.Tasks.Add(instanceTask); 
            }

            _context.ProcessInstances.Add(instance);
            await _context.SaveChangesAsync();

            return instance; 
        }
    }
    
    
}
