using System.ComponentModel.DataAnnotations;

namespace VendorProcessManagerV1.Models
{
    /// <summary>
    /// Represents a transition from a template task to the next task. Each task may have one or more transitions.
    /// </summary>
    public class ProcessTemplateTransition
    {
        /// <summary>
        /// Unique identifier for the transition
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Indicates the task that this transition leads from. Required field. 
        /// </summary>
        [Required]
        public Guid FromProcessTemplateTaskId { get; set; }

        /// <summary>
        /// The task object the transition leads from. 
        /// </summary>
        public ProcessTemplateTask FromProcessTemplateTask { get; set; }

        /// <summary>
        /// The task the transition leads to. 
        /// </summary>
        public Guid? ToProcessTemplateTaskId { get; set; }

        /// <summary>
        /// The task object the transition leads to.
        /// </summary>
        public ProcessTemplateTask? ToProcessTemplateTask { get; set; }

        /// <summary>
        /// The display label for the transition. Required field.
        /// </summary>
        [Required]
        public string DisplayLabel { get; set; }

        /// <summary>
        /// The sort order for the transition. Used if several transitions leads away from a task. 
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Condition type for using this transition.
        /// </summary>
        public string? ConditionType { get; set; }

        /// <summary>
        /// Conition expression for transition
        /// </summary>
        public string? ConditionExpression  { get; set; }

        /// <summary>
        /// Boolean value to mark if the transition is the default transition.
        /// </summary>
        public bool IsDefault { get; set; } = false;
        
    }
}
