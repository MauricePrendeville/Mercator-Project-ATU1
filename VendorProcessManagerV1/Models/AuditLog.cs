namespace VendorProcessManagerV1.Models
{
    public class AuditLog
    { 
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

    }
}
