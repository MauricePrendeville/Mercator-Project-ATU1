using Microsoft.AspNetCore.Mvc;

namespace VendorProcessManagerV1.Controllers
{
    public class AppUsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
