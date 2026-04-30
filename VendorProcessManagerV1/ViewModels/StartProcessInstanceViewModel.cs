using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VendorProcessManagerV1.ViewModels
{
    public class StartProcessInstanceViewModel
    {

        public Guid ProcessTemplateId { get; set; }
        public string TemplateName { get; set; }
        public int TaskCount { get; set; }

        [Required]
        public string InstanceName { get; set; }
        public string? SuggestedName { get; set; }

    }
}
