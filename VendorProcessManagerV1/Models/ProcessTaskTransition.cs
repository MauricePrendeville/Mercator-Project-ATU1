namespace VendorProcessManagerV1.Models
{
    public class ProcessTaskTransition
    {
        public Guid Id { get; set; }
        public Guid ProcessInstanceId { get; set; }
        public Guid ProcessTemplateTrasitionId { get; set; }
        public Guid FromProcessStepId { get; set; }
        public Guid ToProcessStepId { get; set; }
        public Guid ActionUserId { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionValue { get; set; }

    }
}
