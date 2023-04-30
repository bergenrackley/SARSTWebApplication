using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SARSTWebApplication.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Session.GetString("userName") == null)
            {
                RedirectToAction("Index", "Public");
                filterContext.Result = RedirectToAction("Index", "Public");
                return;
            }
        }

    }
}
