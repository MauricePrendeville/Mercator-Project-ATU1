using System.ComponentModel.DataAnnotations; 

namespace VendorProcessManagerV1.Models
{
    /// <summary>
    /// Represents a single task that is associated with a process template
    /// </summary>
    public class ProcessTemplateTask
    {
        /// <summary>
        /// The unique identifier for the template task
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The identifier for the parent process template for this task.
        /// </summary>
        public Guid ProcessTemplateId { get; set; }

        /// <summary>
        /// The process template object
        /// </summary>
        public ProcessTemplate ProcessTemplate { get; set; }
        //public Guid TaskId { get; set; }

        /// <summary>
        /// The title of the task. This field is required.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The description of the task
        /// </summary>
        public string? Description { get; set; }
        //public string? ApproverId   { get; set; }
        //public AppUser? Approver { get; set; }

        /// <summary>
        /// The name of the team that approve this task.
        /// </summary>        
        public string? ApproverTeam { get; set; }

        /// <summary>
        /// Boolean to indicate if approval is required for this task
        /// </summary>
        public bool ApprovalRequired { get; set; }

        /// <summary>
        /// An integer to indicate the sort order of the task. This is user defined.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// The default owner role for the task
        /// </summary>
        public string DefaultOwnerRole {  get; set; }

        /// <summary>
        /// List of tasks that precede this task
        /// </summary>
        public List<ProcessTemplateTask>? DependsOn { get; set; }
        
        /// <summary>
        /// The list of tasks that are the next step in the process 
        /// </summary>
        public List<ProcessTemplateTask>? SuccessorTasks { get; set; }
    
        /// <summary>
        /// List of transitions to the next task in the process.
        /// </summary>
        public ICollection<ProcessTemplateTransition> Transitions  { get; set; } 
            = new List<ProcessTemplateTransition>();
    }
}
