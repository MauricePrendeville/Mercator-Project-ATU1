namespace VendorProcessManagerV1.Models
{
    /// <summary>
    /// Live tasks required to be completed for the Instance to be successful
    /// </summary>
    public class ProcessTask
    {
        public Guid Id { get; set; }

        //Parent Instance for this task
        public Guid ProcessInstanceId { get; set; }
        public ProcessInstance ProcessInstance { get; set; }

        
        //Template task this one was copied from
        public Guid ProcessTemplateTaskId { get; set; }
        public ProcessTemplateTask ProcessTemplateTask { get; set; }


        //basic text description copied from Template
        public string Title { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }

        //Notes for this specific task
        public string? TaskNotes { get; set; }


        //ownership details
        public string? CreatorId { get; set; }
        public AppUser? Creator { get; set; }
        public string? OwnerId { get; set; }
        public AppUser? Owner { get; set; }


        //approval details
        public string? ApproverTeam { get; set; }
        public string? ApproverId { get; set; }
        public AppUser? Approver { get; set; }
        public bool ApprovalRequired { get; set; }
        public ApproveStatus? ApproveStatus { get; set; } //change to Enum

        //Dates
        public DateTime? StartedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }   
        public DateTime? ApproveDate { get; set; }
       
        public ProcessTaskStatus ProcessTaskStatus { get; set; }
        //public List<ProcessTask>? DependsOn { get; set; } //may replace with dependency table
        //public List<ProcessTask>? SuccessorTasks { get; set; }
        public bool IsCompleted { get; set; }
       

    }

    public enum ProcessTaskStatus
    {
        NotStarted, 
        InProgress, 
        Completed,
        Approved,
        Rejected, 
        Skipped
    }

    public enum ApproveStatus
    {
        NotRequired,
        Approved, 
        Pending,
        Rejected
    }
}
