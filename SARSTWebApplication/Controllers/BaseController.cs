using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SARSTWebApplication.Data;

namespace SARSTWebApplication.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Session.GetString("userName") == null)
            {
                RedirectToAction("Index", "Account");
                filterContext.Result = RedirectToAction("Index", "Account");
                return;
            }
        }

    }
}
