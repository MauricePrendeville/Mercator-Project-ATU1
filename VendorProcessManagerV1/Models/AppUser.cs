using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace VendorProcessManagerV1.Models
{
    public class AppUser : IdentityUser
    {
        //Guid Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50)]
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        //public string? Email { get; set; }
        public string? Team { get; set; } //create Team object
        public string? UserType { get; set; }


    }
}
