namespace VendorProcessManagerV1.Models
{
    public class ProcessTemplateTransition
    {
        public Guid Id { get; set; }
        public Guid ProcessTemplateId { get; set; }
        public Guid FromProcessTemplateTaskId { get; set; }
        public Guid ToProcessTemplateTaskId { get; set; }
        public string DisplayLabel { get; set; }
        public int SortOrder { get; set; }
        public string? ConditionType { get; set; }
        public int? ConditionExpression  { get; set; }
        public bool? IsDefault { get; set; }
        
    }
}
