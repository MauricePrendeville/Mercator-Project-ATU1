using Microsoft.AspNetCore.Mvc.Rendering;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.ViewModels
{
    public class ProcessInstanceIndexViewModel
    {
        public string SortOrder { get; set; }
        public string TeamFilter { get; set; }
        public string TemplateFilter { get; set; }
        public string VendorFilter { get; set; }
        public string StatusFilter { get; set; }
        public string InitiatorFilter { get; set; }



        public string InstanceNameSort { get; set; }
        public string TemplateSort { get; set; }
        public string VendorSort { get; set; }
        public string DateSort { get;  set; }
        public string InitiatorSort { get; set; }
        public string StatusSort { get; set; }

        public IEnumerable<SelectListItem> TeamList { get; set; }
        public IEnumerable<SelectListItem> VendorList { get; set; }
        public IEnumerable<SelectListItem> TemplateList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> InitiatorList { get; set; }

        public IEnumerable<ProcessInstance> Instances { get; set; }        
    }
}
