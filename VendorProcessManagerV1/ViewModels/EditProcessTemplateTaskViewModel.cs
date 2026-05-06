using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VendorProcessManagerV1.ViewModels
{
    public class EditProcessTemplateTaskViewModel
    {
        public Guid Id { get; set; }
        public Guid ProcessTemplateId { get; set; }
        
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ApproverTeam { get; set; }
        public SelectList? ApproverTeamOptions { get; set; }
        public bool ApprovalRequired { get; set; }
        public int SortOrder { get; set; }
        public string? DefaultOwnerRole {  get; set; }
    }
}
