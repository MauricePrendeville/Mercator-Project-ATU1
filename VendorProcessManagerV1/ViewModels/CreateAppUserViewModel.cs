using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace VendorProcessManagerV1.ViewModels
{
    public class CreateAppUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? Team { get; set; }
        public string? UserType { get; set; }
       
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        public SelectList? TeamOptions { get; set; }
        public SelectList? UserTypeOptions { get; set; }

    }
}
