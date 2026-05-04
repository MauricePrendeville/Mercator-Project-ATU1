using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.ViewModels
{
    public class EditProcessInstanceViewModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public string? TemplateName { get; set; }
        public string? VendorName { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? TargetEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }

        [Required]
        public ProcessInstanceStatus Status { get; set; }
        public SelectList? StatusOptions { get; set; }
    }
}
