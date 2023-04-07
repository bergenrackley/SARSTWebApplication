using Microsoft.AspNetCore.Mvc;

namespace SARSTWebApplication.Controllers
{
    public class ResidentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
