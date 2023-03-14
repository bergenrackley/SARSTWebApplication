using Microsoft.AspNetCore.Mvc;

namespace SARSTWebApplication.Controllers
{
    public class ServiceTrackerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
