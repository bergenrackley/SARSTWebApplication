using Microsoft.AspNetCore.Mvc;

namespace SARSTWebApplication.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
