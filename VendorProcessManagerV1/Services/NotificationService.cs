using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context; 
        
        public NotificationService(ApplicationDbContext context)
        {
            _context = context; 
        }
        
        public async Task<int> GetPendingApprovalCountAsync (string userId)
        {

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || string.IsNullOrEmpty(user.Team))
                return 0;

            return await _context.ProcessTasks
                .Include(t => t.ProcessInstance)
                .CountAsync(t =>
                    t.IsActive &&
                    t.ApprovalRequired &&
                    t.ApproveStatus == ApproveStatus.Pending &&
                    t.ApproverTeam == user.Team &&
                    t.ProcessInstance.Status == ProcessInstanceStatus.InProgress);
        }
        public async Task<List<PendingApprovalItem>> GetPendingApprovalsAsync(
            string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || string.IsNullOrEmpty(user.Team))
                return new List<PendingApprovalItem>();

            return await _context.ProcessTasks
                .Include(t => t.ProcessInstance)
                    .ThenInclude(i => i.VendorCandidate)
                .Where(t =>
                    t.IsActive &&
                    t.ApprovalRequired &&
                    t.ApproveStatus == Models.ApproveStatus.Pending &&
                    t.ApproverTeam == user.Team &&
                    t.ProcessInstance.Status == Models.ProcessInstanceStatus.InProgress)
                .OrderBy(t => t.StartedDate)
                .Select(t => new PendingApprovalItem
                {
                    TaskId = t.Id,
                    TaskTitle = t.Title,
                    ProcessInstanceId = t.ProcessInstanceId,
                    InstanceName = t.ProcessInstance.InstanceName,
                    VendorName = t.ProcessInstance.VendorCandidate.Name,
                    StartedDate = t.StartedDate
                })
                .ToListAsync();
        }    
    }    
}
