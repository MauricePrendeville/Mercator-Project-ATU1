using Microsoft.AspNetCore.Identity;
namespace VendorProcessManagerV1.Models
{
    public class User : IdentityUser
    {
        //Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        //public string? Email { get; set; }
        public string? Team { get; set; } //create Team object
        public string? UserType { get; set; }


    }
}
