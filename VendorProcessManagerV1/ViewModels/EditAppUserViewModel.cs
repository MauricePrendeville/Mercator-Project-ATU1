using Microsoft.AspNetCore.Mvc.Rendering;

namespace VendorProcessManagerV1.ViewModels
{
    public class EditAppUserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string Email { get; set; }
        public string? Team {  get; set; }
        public string? UserType { get; set; }
        public SelectList? TeamOptions { get; set; }
        public SelectList? UserTypeOptions { get; set; }

    }
}
