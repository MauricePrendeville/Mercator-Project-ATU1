using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.ViewModels
{
    public class CreateProcessTemplateTaskViewModel
    {
        
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }
        
        public string ApproverTeam { get; set; }

        public bool ApprovalRequired { get; set; }
        public int SortOrder { get; set; }
        public string DefaultOwnerRole { get; set; }
        public string? TemplateName { get; set; }
        public Guid ProcessTemplateId { get; set; }
        public ProcessTemplate ProcessTemplate { get; set; }
        public ICollection<ProcessTaskTransition> TaskTransitions { get; set; }
        
    }
}