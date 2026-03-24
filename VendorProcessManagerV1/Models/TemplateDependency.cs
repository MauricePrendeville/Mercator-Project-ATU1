namespace VendorProcessManagerV1.Models
{
    public class TemplateDependency
    {
        public Guid Id { get; set; }
        public Guid PredecessorTemplateTaskId { get; set; }
        public Guid SuccessorTemplateTaskId { get; set; }
        public string DependencyType { get; set; }
        public bool ApprovalRequired { get; set; }

    }
}
