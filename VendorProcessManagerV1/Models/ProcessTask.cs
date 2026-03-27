namespace VendorProcessManagerV1.Models
{
    public class ProcessTask
    {
        public Guid Id { get; set; }
        public string Task_Id { get; set; }
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
        public List<ProcessTask>? DependsOn { get; set; } //may replace with dependency table
        public List<ProcessTask>? SuccessorTasks { get; set; }
        public bool IsCompleted { get; set; }
        public bool RequiresApproval { get; set; }

    }
}
