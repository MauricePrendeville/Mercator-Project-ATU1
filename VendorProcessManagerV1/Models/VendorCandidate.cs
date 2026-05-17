namespace VendorProcessManagerV1.Models
{
    /// <summary>
    /// Represents a vendor candidate that can be added to a process instance.
    /// </summary>
    public class VendorCandidate
    { 
        /// <summary>
        /// Unique identifier for the vendor.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the vendor candidate.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The category of the vendor candidate.
        /// </summary>
        public string Category { get; set; } //split vendor category off into its own table
                                             //so a vendor can have more than one category
        
        /// <summary>
        /// The create date of the vendor candidate object.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// The last modified date. 
        /// </summary>
        public DateTime? LastModifiedDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// The identifier for the owner of the vendor candidate record.
        /// </summary>
        public string? OwnerId {  get; set; }

        /// <summary>
        /// The owner object. 
        /// </summary>
        public AppUser? Owner { get; set; }

        /// <summary>
        /// The payment terms that are associated with the vendor. 
        /// </summary>
        public string PaymentTerms { get; set; } //update to ENUM

        /// <summary>
        /// The list of process instance that the vendor is associated with.
        /// </summary>
        public ICollection<ProcessInstance> ProcessInstances { get; set; }
            = new List<ProcessInstance>();

    }
}
