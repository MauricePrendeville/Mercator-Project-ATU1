namespace VendorProcessManagerV1.DTO
{
    public class GanttDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Progress { get; set; }
        public string Dependencies { get; set; }
        public string CustomClass { get; set; }

    }
}
