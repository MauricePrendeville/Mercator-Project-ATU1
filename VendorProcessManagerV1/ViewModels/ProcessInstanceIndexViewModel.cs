using Microsoft.AspNetCore.Mvc.Rendering;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.ViewModels
{
    public class ProcessInstanceIndexViewModel
    {
        public string SortOrder { get; set; }
        public string TeamFilter { get; set; }

        public IEnumerable<SelectListItem> TeamList { get; set; }
        public IEnumerable<ProcessInstance> Instances { get; set; }

        public string InstanceNameSort { get; set; }
        public string TemplateSort { get; set; }
        public string VendorSort { get; set; }
        public string DateSort { get;  set; }
        public string InitiatorSort { get; set; }
        public string StatusSort { get; set; }
    }
}
