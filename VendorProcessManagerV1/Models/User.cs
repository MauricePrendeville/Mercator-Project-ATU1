namespace VendorProcessManagerV1.Models
{
    public class User
    {
        Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? Email { get; set; }
        public string? Team { get; set; } //create Team object
        public string? UserType { get; set; }


    }
}
