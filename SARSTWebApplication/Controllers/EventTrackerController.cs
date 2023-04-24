using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using SARSTWebApplication.Data;
using SARSTWebApplication.Enums;
using SARSTWebApplication.Models;
using System;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace SARSTWebApplication.Controllers
{
    public class EventTrackerController : BaseController
    {
        private AppDbContext _dbContext;
        public EventTrackerController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection")); //creates sql db connection
        }

        public IActionResult Index()
        {
            return View(); //page with buttons for prompting user to select Service or Disciplinary. Both buttons navigate to SelectResident, with a url parameter "type" telling the SelectResident page where to go next
        }

        public IActionResult SelectResident(string type) {
            TempData["eventType"] = type; //used for making the page redirect work
            return View();
        }

        [HttpGet]
        public PartialViewResult SearchResidents(string query) { //this partial view gets called by ajax, creates teh table seen in selectresident. does the searching and refreshes the partialview on keyup event from seach box
            List<Resident> result = _dbContext.Database.SqlQuery<Resident>("Select * from dbo.residents where residentId in (Select residentId from dbo.residentStays where CheckOutDateTime is NULL) and (firstName + ' ' + lastName like @query or distinguishingFeatures like @query)", new SqlParameter("@query", "%" +  query + "%")).ToList();
            return PartialView("_GridView", result);
        }

        public IActionResult ServiceForm(string id) //id is the resident id
        {
            ViewBag.ServiceList = ServicesToList(_dbContext.ServicesOffered.ToList()); //get all services offered from ServicesOffered table, formats serialied list into dropdown id/value pairs
            ViewBag.residentId = id; //set residentId
            ViewBag.currentUserName = HttpContext.Session.GetString("userName"); //get username from session data
            ViewBag.stayId = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckOutDateTime is NULL and residentId='" + id + "'").ToList().First().stayId; //get current stay for resident
            return View();
        }

        [HttpPost]
        public string CheckServiceEvent(ServiceEvent serviceEvent) { //when the user clicks create, calls this with ajax. 
            if (ModelState.IsValid) return "Valid"; //checks validity of model data (required fields, etc) if valid, return true
            else return ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToJson(); //if invalid, get the parts of the model that are malformed and return them as json
        }

        [HttpPost]
        public IActionResult SubmitServiceForm(ServiceEvent serviceEvent) //called by page itself
        {
            _dbContext.ServiceTracker.Add(serviceEvent); //add model
            _dbContext.SaveChanges(); //save changes
            return RedirectToAction("Index"); //redirect to Event index
        }

        public IActionResult DisciplinaryForm(string id) //id is resident id
        {
            ViewBag.DisciplinaryTypes = getDisciplinaryTypes(); //gets and formats DisciplinaryTypes enum as dropdown id/value pair
            ViewBag.residentId = id;
            ViewBag.currentUserName = HttpContext.Session.GetString("userName");
            ViewBag.stayId = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckOutDateTime is NULL and residentId='" + id + "'").ToList().First().stayId; //second verse, same as the first
            return View();
        }

        [HttpPost]
        public string CheckDisciplinaryEvent(DisciplinaryEvent disciplinaryEvent)  //when the user clicks create, calls this with ajax. 
        {
            if (ModelState.IsValid) return "Valid"; //valid
            else return ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToJson(); //malformed as json
        }

        [HttpPost]
        public RedirectToActionResult SubmitDisciplinaryForm(DisciplinaryEvent disciplinaryEvent) //called by page when valid
        {
            _dbContext.DisciplinaryTracker.Add(disciplinaryEvent);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [NonAction]
        public SelectList ServicesToList(List<Service> table) //formats Services table as valid dropdown enumerable
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

        [NonAction]
        public List<SelectListItem> getDisciplinaryTypes() //formats DisciplinaryTypes Enum as valid dropdown enumerable
        {
            return Enum.GetValues(typeof(DisciplinaryTypes)).Cast<DisciplinaryTypes>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
        }
    }
}
