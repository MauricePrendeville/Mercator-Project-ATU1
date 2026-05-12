namespace VendorProcessManagerV1.DTO
{   
    /// <summary>
    /// A data transfer object to pass information to the Frappe Gantt chart system.
    /// parameters are in camelCase as PascalCase doesn't work with Frappe
    /// </summary>
    public class GanttDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int progress { get; set; }
        public string dependencies { get; set; }
        public string customClass { get; set; }

    }
}
