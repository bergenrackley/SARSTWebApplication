using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using SARSTWebApplication.Data;
using SARSTWebApplication.Enums;
using SARSTWebApplication.Models;
using System.Data;
using System.Net;

namespace SARSTWebApplication.Controllers
{
    public class EventTrackerController : Controller
    {
        private AppDbContext _dbContext;
        public EventTrackerController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectResident(string type) {
            ViewBag.eventType = type;
            var command = _dbContext.Database.SqlQuery<Resident>("Select * from dbo.residents where residentId in (Select residentId from dbo.residentStays where CheckOutDateTime is NULL)").ToList();
            return View(command);
        }

        public IActionResult ServiceForm(string id)
        {
            ViewBag.ServiceList = ServicesToList(_dbContext.ServicesOffered.ToList());
            ViewBag.residentId = id;
            ViewBag.currentUserName = HttpContext.Session.GetString("userName");
            return View();
        }

        public string SubmitServiceForm(ServiceEvent serviceEvent)
        {
            /*
            _dbContext.ServiceTracker.Add(serviceEvent);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
            */

            return serviceEvent.ToJson();
        }

        public IActionResult DisciplinaryForm(string id)
        {
            ViewBag.DisciplinaryTypes = getUserTypes();
            ViewBag.residentId = id;
            ViewBag.currentUserName = HttpContext.Session.GetString("userName");
            return View();
        }

        [NonAction]
        public SelectList ServicesToList(List<Service> table)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (Service row in table)
            {
                list.Add(new SelectListItem()
                {
                    Text = row.serviceName.ToString(),
                    Value = row.serviceName.ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        public List<SelectListItem> getUserTypes()
        {
            return Enum.GetValues(typeof(DisciplinaryTypes)).Cast<DisciplinaryTypes>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
        }
    }
}
