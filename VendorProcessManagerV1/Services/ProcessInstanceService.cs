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
                //Name = instanceName, 
                InitiatedById = initiatedById, 
                CreatedDate = DateTime.Now, 
                Status = ProcessInstanceStatus.NotStarted
            };

            foreach (var templateTask in template.Tasks.OrderBy(t => t.SortOrder))
            {
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
                    ProcessTaskStatus = ProcessTaskStatus.NotStarted,
                    StartedDate = null,
                    CompletedDate = null
                };

                instance.Tasks.Add(instanceTask); 
            }

            _context.ProcessInstances.Add(instance);
            await _context.SaveChangesAsync();

            return instance; 
        }
    }
    
    
}
