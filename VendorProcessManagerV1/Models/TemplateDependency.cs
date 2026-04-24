namespace VendorProcessManagerV1.Models
{
    public class TemplateDependency
    {
        public Guid Id { get; set; }
        public Guid PredecessorTemplateTaskId { get; set; }
        public Guid SuccessorTemplateTaskId { get; set; }
        public string DependencyType { get; set; } //add Enum for the types
        public bool ApprovalRequired { get; set; }

    }
}
