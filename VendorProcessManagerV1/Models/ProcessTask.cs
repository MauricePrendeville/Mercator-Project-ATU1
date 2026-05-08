namespace VendorProcessManagerV1.Models
{
    /// <summary>
    /// Details of a single task that is part of a process instance.
    /// </summary>
    public class ProcessTask
    {
        /// <summary>
        /// Unique identifier for this task. 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique identifier for the parent instance for this task
        /// </summary>
        public Guid ProcessInstanceId { get; set; }
        /// <summary>
        /// The details of the parent process instance.
        /// </summary>
        public ProcessInstance ProcessInstance { get; set; }
        /// <summary>
        /// The unique identifier for the template task this task was based on.
        /// </summary>        
        public Guid ProcessTemplateTaskId { get; set; }
        /// <summary>
        /// The details fo the template task this task was based on. 
        /// </summary>
        public ProcessTemplateTask ProcessTemplateTask { get; set; }
        /// <summary>
        /// The title of the task is copied from the title of the template task.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// This description is copied frin the template description. 
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// The sorting order for the task. It is copied from the template. 
        /// </summary>
        public int SortOrder { get; set; }
        /// <summary>
        /// The user added notes for the task.
        /// </summary>                
        public string? TaskNotes { get; set; }


        /// <summary>
        /// Unique identifier for the task creator.
        /// </summary>
        public string? CreatorId { get; set; }
        /// <summary>
        /// Details of the task creator.
        /// </summary>
        public AppUser? Creator { get; set; }
        /// <summary>
        /// Unique identifier of the the task owner. 
        /// </summary>
        public string? OwnerId { get; set; }
        /// <summary>
        /// Details of the task owner. 
        /// </summary>
        public AppUser? Owner { get; set; }
        
        /// <summary>
        /// The name of the team whose members can approve this task.
        /// </summary>
        public string? ApproverTeam { get; set; }
        /// <summary>
        /// Unique identifier of the user who approved this task. 
        /// </summary>
        public string? ApproverId { get; set; }
        /// <summary>
        /// Details of the user that approved this task. 
        /// </summary>
        public AppUser? Approver { get; set; }
        /// <summary>
        /// Boolean value to mark if this task requires approval.
        /// </summary>
        public bool ApprovalRequired { get; set; }
        /// <summary>
        /// The approval status of this task.
        /// </summary>
        public ApproveStatus? ApproveStatus { get; set; } 

        /// <summary>
        /// The date the task was started.
        /// </summary>
        public DateTime? StartedDate { get; set; }
        /// <summary>
        /// Date the task was updated. 
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        /// <summary>
        /// Date the task was set to completed.
        /// </summary>
        public DateTime? CompletedDate { get; set; }   
        /// <summary>
        /// Date the task was approved.
        /// </summary>
        public DateTime? ApproveDate { get; set; }
        
        /// <summary>
        /// Status of the task. 
        /// </summary>
        public ProcessTaskStatus ProcessTaskStatus { get; set; }
        /// <summary>
        /// Boolean value to signifying if the task is completed. 
        /// </summary>
        public bool IsCompleted { get; set; }
        /// <summary>
        /// Boolean value to signify if the task is active. 
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Unique identifier to record the transition that led to this task.
        /// </summary>
        public Guid? ActivatedByTransitionId { get; set; }
        /// <summary>
        /// Unique identifier to record what transition was selected after this task.
        /// </summary>
        public Guid? SelectedTransitionId { get; set; }
       
    }

    /// <summary>
    /// List of possible task statuses. 
    /// </summary>
    public enum ProcessTaskStatus
    {
        NotStarted, 
        InProgress, 
        Completed,   
        Approved, 
        Rejected,
        Skipped
    }

    /// <summary>
    /// list of possible approval statuses. 
    /// </summary>
    public enum ApproveStatus
    {
        NotRequired,
        Approved, 
        Pending,
        Rejected
    }
}
