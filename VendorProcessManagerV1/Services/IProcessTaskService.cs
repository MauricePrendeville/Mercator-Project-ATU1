using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.Services
{
    public interface IProcessTaskService
    {
        Task<CompleteTaskResult> CompleteTaskAsync(
            Guid taskId,
            Guid? selectedTransitionId,
            string completedById);

        Task<List<ProcessTemplateTransition>> GetAvailableTransitionsAsync(
            Guid taskId);

        Task<bool> CanStartTaskAsync(Guid taskId); 
    }

    public class CompleteTaskResult
    {
        public bool Succeeded { get; set; }
        public string? ErrorMessage { get; set; }
        public Guid? NextTaskId { get; set; }
        public bool IsProcessComplete { get; set; }
    }
}
