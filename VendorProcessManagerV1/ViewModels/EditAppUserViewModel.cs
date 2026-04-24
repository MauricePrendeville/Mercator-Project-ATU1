using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VendorProcessManagerV1.ViewModels
{
    public class EditAppUserViewModel
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? Team {  get; set; }
        public string? UserType { get; set; }
        
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        public string? ConfirmNewPassword { get; set; }
        public SelectList? TeamOptions { get; set; }
        public SelectList? UserTypeOptions { get; set; }

    }
}
