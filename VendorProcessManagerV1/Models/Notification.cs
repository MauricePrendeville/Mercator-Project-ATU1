namespace VendorProcessManagerV1.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid RecipientId { get; set; }
        public Guid TaskId { get; set; }
        public string Type { get; set; }
        public string Channel {  get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }

    }
}
