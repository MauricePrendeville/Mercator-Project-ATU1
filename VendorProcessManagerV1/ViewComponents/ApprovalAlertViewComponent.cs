using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            if (!User.Identity?.IsAuthenticated ?? true)
                return Content(string.Empty);

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null || string.IsNullOrEmpty(user.Team))
                return Content(string.Empty);

            var items = await _notificationService
                .GetPendingApprovalsAsync(user.Id);

            return View(items); 
        }
    }
}
