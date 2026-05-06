using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.ViewModels
{
    public class ApproveTaskViewModel
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string? ApproverTeam { get; set; }
        public Guid ProcessInstanceId { get; set; }

        [Required]
        public ApproveStatus Decision {  get; set; }
        public SelectList? DecisionOptions { get; set; }

        public string? Notes {  get; set; }
    }
}
