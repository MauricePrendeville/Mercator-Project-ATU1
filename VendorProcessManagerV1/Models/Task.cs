namespace VendorProcessManagerV1.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Guid Creator {  get; set; }
        public Guid Owner { get; set; }
        public string? ApproverTeam { get; set; }
        public Guid? Approver {  get; set; }
        public DateTime? ApproveDate { get; set; }
        public string? ApproveStatus { get; set; } //change to Enum
        public string TaskStatus { get; set; }
        public List<Task>? DependsOn { get; set; }
        public List<Task>? SuccessorTasks { get; set; }
        public bool IsCompleted { get; set; }
        public bool RequiresApproval { get; set; }

    }
}
