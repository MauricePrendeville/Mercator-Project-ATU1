namespace VendorProcessManagerV1.Models
{
    public class VendorCandidate
    { 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; } //split vendor category off into its own table
                                             //so a vendor can have more than one category
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; } = DateTime.MinValue;
        public Guid Creator {  get; set; }
        public string PaymentTerms { get; set; } //update to ENUM

        public ICollection<ProcessInstance> ProcessInstances { get; set; }
            = new List<ProcessInstance>();

    }
}
