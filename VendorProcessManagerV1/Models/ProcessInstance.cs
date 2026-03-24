namespace VendorProcessManagerV1.Models
{
    public class ProcessInstance
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public Guid VendorCandidateId { get; set; }
        public Guid InitiatedBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetEndDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string Status { get; set; }
    }
}
