using Microsoft.AspNetCore.Mvc.Rendering;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.ViewModels
{
    public class ProcessTemplateIndexViewModel
    {
        public string? SortOrder { get; set; }
        public string? SelectedTemplateName { get; set; }
        public string? CategoryFilter { get; set; }
        public string? ActiveFilter { get; set; }

        public string? TemplateNameSort { get; set; }
        public string? DescriptionSort { get; set; }
        public string? CategorySort { get; set; }
        public string? CreatorSort { get; set; }
        public string? ActiveSort { get; set; }

        public IEnumerable<SelectListItem>? TemplateList { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public IEnumerable<SelectListItem>? ActiveList { get; set; }

        public List<ProcessTemplate>? Templates { get; set; }

    }
}
