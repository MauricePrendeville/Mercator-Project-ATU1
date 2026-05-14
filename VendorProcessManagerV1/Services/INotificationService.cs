namespace VendorProcessManagerV1.Services
{
    public interface INotificationService
    {
        Task<int> GetPendingApprovalCountAsync(string userId);
        Task<List<PendingApprovalItem>> GetPendingApprovalsAsync(string userId);
    }

    public class PendingApprovalItem
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; }
        public Guid ProcessInstanceId { get; set; }
        public string InstanceName { get; set; }
        public string VendorName { get; set; }
        public DateTime? StartedDate { get; set; }
    }
}