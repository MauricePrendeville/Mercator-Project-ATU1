using Microsoft.AspNetCore.Mvc.Rendering;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.ViewModels
{
    public class EditProcessTaskViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public bool ApprovalRequired { get; set; }
        public string? ApproverTeam { get; set; }
        public Guid ProcessInstanceId { get; set; }


        public string? OwnerId { get; set; }
        public SelectList? OwnerOptions { get; set; }
        public string? TaskNotes { get; set; }
        public DateTime? StartedDate { get; set; }
        public ProcessTaskStatus ProcessTaskStatus { get; set; }
        public SelectList? StatusOptions { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? ApproverId { get; set; }
        public SelectList? ApproverOptions { get; set; }
        public ApproveStatus? ApproveStatus     { get; set; }
        public SelectList? ApproveStatusOptions { get; set; }
        public DateTime? ApproveDate { get; set; }

    }
}
