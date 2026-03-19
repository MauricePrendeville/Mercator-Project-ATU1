namespace VendorProcessManagerV1.Models
{
    public class VendorCandidate
    { 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; } = DateTime.MinValue;
        public Guid Creator {  get; set; }

    }
}
