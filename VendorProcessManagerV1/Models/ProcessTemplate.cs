using System.Runtime.InteropServices.Marshalling;

namespace VendorProcessManagerV1.Models
{
    /// <summary>
    /// Represents a template for a process.
    /// </summary>
    public class ProcessTemplate
    {
        /// <summary>
        /// Unique identifier for the template
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The name of the template
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the template's process
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The category of the template
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// The identifier for the creator
        /// </summary>
        public string CreatorId { get; set; }
        
        /// <summary>
        /// The creator object
        /// </summary>
        public AppUser? Creator {  get; set; }
        
        /// <summary>
        /// The create date for the template. Set to the current date when the template is created. 
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Boolean value to mark if the template is acive or not
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// A version number for the template
        /// </summary>
        public string? Version { get; set; }
        
        /// <summary>
        /// A list of the template tasks attached to this process template
        /// </summary>
        public ICollection<ProcessTemplateTask> Tasks { get; set; } 
            = new List<ProcessTemplateTask>();
    }
}
