using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using SARSTWebApplication.Data;
using SARSTWebApplication.Enums;
using SARSTWebApplication.Models;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;

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
            ViewBag.stayId = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckOutDateTime is NULL and residentId='" + id + "'").ToList().First().stayId;
            return View();
        }

        [HttpPost]
        public string CheckServiceEvent(ServiceEvent serviceEvent) {
            if (ModelState.IsValid) return "Valid";
            else return ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToJson();
        }

        [HttpPost]
        public IActionResult SubmitServiceForm(ServiceEvent serviceEvent)
        {
            _dbContext.ServiceTracker.Add(serviceEvent);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DisciplinaryForm(string id)
        {
            ViewBag.DisciplinaryTypes = getDisciplinaryTypes();
            ViewBag.residentId = id;
            ViewBag.currentUserName = HttpContext.Session.GetString("userName");
            ViewBag.stayId = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckOutDateTime is NULL and residentId='" + id + "'").ToList().First().stayId;
            return View();
        }

        [HttpPost]
        public string CheckDisciplinaryEvent(DisciplinaryEvent disciplinaryEvent)
        {
            if (ModelState.IsValid) return "Valid";
            else return ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToJson();
        }

        [HttpPost]
        public RedirectToActionResult SubmitDisciplinaryForm(DisciplinaryEvent disciplinaryEvent)
        {
            _dbContext.DisciplinaryTracker.Add(disciplinaryEvent);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
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

        public List<SelectListItem> getDisciplinaryTypes()
        {
            return Enum.GetValues(typeof(DisciplinaryTypes)).Cast<DisciplinaryTypes>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
        }
    }
}
