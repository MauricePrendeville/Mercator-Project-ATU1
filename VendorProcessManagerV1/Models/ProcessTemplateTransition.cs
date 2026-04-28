using System.ComponentModel.DataAnnotations;

namespace VendorProcessManagerV1.Models
{
    public class ProcessTemplateTransition
    {
        public Guid Id { get; set; }
        [Required]
        public Guid FromProcessTemplateTaskId { get; set; }
        public ProcessTemplateTask FromProcessTemplateTask { get; set; }
        public Guid ToProcessTemplateTaskId { get; set; }
        public ProcessTemplateTask ToProcessTemplateTask { get; set; }
        [Required]
        public string DisplayLabel { get; set; }
        public int SortOrder { get; set; }
        public string? ConditionType { get; set; }
        public string? ConditionExpression  { get; set; }
        public bool IsDefault { get; set; } = false;
        
    }
}
