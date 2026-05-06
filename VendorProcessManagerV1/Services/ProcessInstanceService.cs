using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.Services
{
    public class ProcessInstanceService : IProcessInstanceService
    {
        private readonly ApplicationDbContext _context; 

        public ProcessInstanceService(ApplicationDbContext context)
        {
            _context = context; 
        }

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
                Status = ProcessInstanceStatus.NotStarted
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
                    OwnerId = initiatedById
                };

                instance.Tasks.Add(instanceTask); 
            }

            _context.ProcessInstances.Add(instance);
            await _context.SaveChangesAsync();

            return instance; 
        }
    }
    
    
}
