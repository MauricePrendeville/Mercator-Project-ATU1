using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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

            //var allActive = await _context.ProcessTasks
            //    .Include(t => t.ProcessInstance)
            //    .Where(t => t.IsActive)
            //    .ToListAsync();

            //Debug.WriteLine($"====Filter 1 is Active {allActive.Count} tasks====");

            ////var requireApproval = allActive
            ////    .Where(t => t.ApprovalRequired)
            ////    .ToList();
            ////Debug.WriteLine($"====Filter 2 is Active {requireApproval.Count} tasks===");

            //var pendingStatus = requireApproval
            //    .Where(t => t.ApproveStatus == ApproveStatus.Pending)
            //    .ToList();
            //Debug.WriteLine($"====Filter 3 is Active {pendingStatus.Count} tasks===");

            //var nullStatus = requireApproval
            //    .Where(t => t.ApproveStatus == null)
            //    .ToList();
            //Debug.WriteLine($"====Filter 3b is Active {nullStatus.Count} tasks===");

            //var teamMatch = requireApproval
            //    .Where(t => string.Equals(
            //        t.ApproverTeam, user.Team, StringComparison.OrdinalIgnoreCase))
            //    .ToList();
            //Debug.WriteLine($"====Filter 4 is Active {user.Team}: {teamMatch.Count} tasks===");

            //var liveInstance = requireApproval
            //    .Where(t => t.ProcessInstance?.Status ==
            //                    ProcessInstanceStatus.InProgress)
            //    .ToList();
            ////Debug.WriteLine($"====Filter 5 is Active {liveInstance.Count} tasks===");

            //foreach (var t in requireApproval)
            //{
            //    Debug.WriteLine(
            //        $"====Task: {t.Title}"+
            //        $"ApproveStatus={t.ApproveStatus}"+
            //        $"ApproverTeam='{t.ApproverTeam}"+
            //        $"UserTeam='{user.Team}' " +
            //        $"InstanceStatus= {t.ProcessInstance?.Status}===");
            //}
            var results = await _context.ProcessTasks
                .Include(t => t.ProcessInstance)
                    .ThenInclude(i => i.VendorCandidate)
                .Where(t =>
                    t.IsActive &&
                    t.ApprovalRequired &&
                    (t.ApproveStatus == ApproveStatus.Pending || 
                        t.ApproveStatus == null) &&
                    t.ApproverTeam.ToLower() == user.Team.ToLower() &&
                    (t.ProcessInstance.Status == ProcessInstanceStatus.InProgress || 
                    t.ProcessInstance.Status == ProcessInstanceStatus.NotStarted))
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
           // Debug.WriteLine($"====Final result {results.Count} tasks===");
            return results;
        }    
    }    
}
