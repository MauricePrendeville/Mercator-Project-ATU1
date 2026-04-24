using Microsoft.AspNetCore.Mvc.Rendering;

namespace VendorProcessManagerV1.ViewModels
{
    public class CreateProcessTemplateViewModel
    {
        public Guid Id { get; set; }
        //[Required]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        //public SelectList? Category { get; set; }
        //public string CreatorId { get; set; }
        public string? Category {  get; set; }
        //public SelectList? CreatorOptions { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public string? Version { get; set; }
        public string? CreatorName { get; set; }
    }
}
