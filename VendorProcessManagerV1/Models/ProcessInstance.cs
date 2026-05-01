namespace VendorProcessManagerV1.Models
{
    /// <summary>
    /// Is a live instance of a process in action
    /// </summary>
    public class ProcessInstance
    {
        public Guid Id { get; set; }

        public string InstanceName { get; set; }

        //link to template
        public Guid ProcessTemplateId { get; set; }
        public ProcessTemplate ProcessTemplate { get; set; }
        public Guid VendorCandidateId { get; set; }
        public VendorCandidate VendorCandidate { get; set; }
        public string InitiatedById { get; set; }
        public AppUser InitiatedBy { get; set; }
        public DateTime CreatedDate { get; set; }   
        public DateTime? StartDate { get; set; }
        public DateTime? TargetEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        
        //Instance status
        public ProcessInstanceStatus Status { get; set; }

        //running tasks
        public ICollection<ProcessTask> Tasks { get; set; }
            = new List<ProcessTask>(); 
    }

    public enum ProcessInstanceStatus
    {
        NotStarted, 
        InProgress, 
        Completed, 
        Cancelled
    }
}
