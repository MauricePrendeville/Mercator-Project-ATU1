using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VendorProcessManagerV1.ViewModels
{
    public class CreateProcessTemplateTransitionViewModel
    {
        //hidden fields
        public Guid FromProcessTemplateTaskId { get; set; }
        public Guid ProcessTemplateId { get; set; } //for redirect

        public string? TaskTitle { get; set; }
        public string? TemplateName { get; set; }
        
        [Required]
        public string DisplayLabel  { get; set; }
        public int SortOrder { get; set; }
        //target task dropdown code
        public Guid? ToProcessTemplateTaskId    { get; set; }
        public SelectList? TargetTaskOptions    { get; set; }

        public string? ConditionType { get; set; }
        public SelectList? ConditionTypeOptions { get; set; } //dropdown
        public string? ConditionExpression { get; set; }    //will change to dropdown

        //Default flag
        public bool IsDefault { get; set; }

        //warning display
        public bool HasExistingDefault { get; set; }
        public string? ExistingDefaultLabel { get; set; }
    }
}
