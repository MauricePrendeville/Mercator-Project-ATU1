using Microsoft.AspNetCore.Mvc.Rendering;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.ViewModels
{
    public class CompleteTaskViewModel
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; }
        public Guid ProcessInstanceId { get; set; }
        public bool HasTransitions { get; set; }
        public Guid? SelectedTransitionId { get; set; }
        public Guid? DefaultTransitionId { get; set; }
        public List<ProcessTemplateTransition> Transitions { get; set; }
            = new List<ProcessTemplateTransition>(); 
    }
}
