using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VendorProcessManagerV1.ViewModels
{
    public class EditProcessTemplateTransitionViewModel
    {
        public Guid Id { get; set; }

        public Guid FromProcessTemplateTaskId { get; set; }
        public Guid ProcessTemplateId { get; set; }
        public string? TaskTitle { get; set; }
        public string? TemplateName { get; set; }

        [Required]
        public string DisplayLabel { get; set; }
        public Guid? ToProcessTemplateTaskId { get; set; }
        public SelectList? TargetTaskOptions { get; set; }
        public int SortOrder { get; set; }
        public string? ConditionType { get; set; }
        public SelectList? ConditionTypeOptions { get; set; }
        public string? ConditionExpression { get; set; }

        public bool IsDefault { get; set; }

        public bool HasExistingDefault { get; set; }
        public string? ExistingDefaultLabel { get; set; }
    }
}
