using System.Runtime.InteropServices;

namespace VendorProcessManagerV1.Models
{
    public class ProcessTemplateTask
    {
        public Guid Id { get; set; }
        public Guid ProcessTemplateId { get; set; }
        public ProcessTemplate ProcessTemplate { get; set; }
        //public Guid TaskId { get; set; }               
        public string Title { get; set; }
        public string Description { get; set; }
        public AppUser ApproverId { get; set; }
        public string ApproverTeam { get; set; }
        public bool ApprovalRequired { get; set; }
        public int SortOrder { get; set; }
        public string DefaultOwnerRole {  get; set; }
        public List<ProcessTemplateTask>? DependsOn { get; set; }
        public List<ProcessTemplateTask>? SuccessorTasks { get; set; }
    
        public ICollection<ProcessTemplateTransition> Transitions  { get; set; } 
            = new List<ProcessTemplateTransition>();
    }
}
