using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.Services;

namespace VendorProcessManagerV1.ViewComponents
{
    public class ApprovalAlertViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;

        public ApprovalAlertViewComponent(
            INotificationService notificationService, 
            UserManager<AppUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var testItems = new List<PendingApprovalItem>
            //{


                if (!User.Identity?.IsAuthenticated ?? true)
                {
                    Debug.WriteLine("Alert: user not authentitcated"); 
                
                    return Content("NotAuth");
                }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user == null)
            {
                Debug.WriteLine("Alert: user is null");
                return Content("No user");
            }

            Debug.WriteLine($"Approval Alert: User team = '{user.Team}'");
            if (string.IsNullOrEmpty(user.Team))
                return Content($"No team for {user.Email}");

            
                //new PendingApprovalItem
                //{
                //    TaskId = Guid.NewGuid(), 
                //    TaskTitle = "Test Alert", 
                //    ProcessInstanceId = Guid.NewGuid(), 
                //    InstanceName = "Alert test Instance", 
                //    VendorName = "test vendor"
                //}
            

            //if (!User.Identity?.IsAuthenticated ?? true)
            //    return Content(string.Empty);

            //var user = await _userManager.GetUserAsync(HttpContext.User);
            //if (user == null || string.IsNullOrEmpty(user.Team))
            //    return Content(string.Empty);

            var items = await _notificationService
                .GetPendingApprovalsAsync(user.Id);

            Debug.WriteLine($"APPROVAL ALERT: Found {items.Count} items");

            return View(items); 
        }
    }
}
