namespace VendorProcessManagerV1.Models
{
    public class ProcessTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public Guid Creator {  get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public string Version { get; set; }

    }
}
