﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using SARSTWebApplication.Data;
using SARSTWebApplication.Enums;
using SARSTWebApplication.Models;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public IActionResult SelectResident(string type)
        {
            TempData["eventType"] = type; //used for making the page redirect work
            TempData.Keep("eventType");
            return View();
        }

        [HttpGet]
        public PartialViewResult SearchResidents(string query)
        { //this partial view gets called by ajax, creates teh table seen in selectresident. does the searching and refreshes the partialview on keyup event from seach box
            List<Resident> result = _dbContext.Database.SqlQuery<Resident>("Select * from dbo.residents where residentId in (Select residentId from dbo.residentStays where CheckOutDateTime is NULL) and (firstName + ' ' + lastName like @query or distinguishingFeatures like @query)", new SqlParameter("@query", "%" + query + "%")).ToList();
            return PartialView("_GridView", result);
        }

        public IActionResult ServiceForm(string id) //id is the resident id
        {
            ViewBag.ServiceList = ServicesToList(_dbContext.Database.SqlQuery<Service>("Select * from dbo.servicesOffered where (endDate >= GETDATE() or endDate is NULL) and (startDate <= GETDATE() or startDate is NULL)").ToList()); //get all services offered from ServicesOffered table, formats serialied list into dropdown id/value pairs
            ViewBag.residentId = id; //set residentId
            ViewBag.currentUserName = HttpContext.Session.GetString("userName"); //get username from session data
            ViewBag.stayId = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckOutDateTime is NULL and residentId=@query", new SqlParameter("@query", id)).ToList().First().stayId; //get current stay for resident
            return View();
        }

        [HttpPost]
        public string SubmitServiceForm(ServiceEvent serviceEvent)
        { //when the user clicks create, calls this with ajax. 
            if (!ModelState.IsValid)
            {
                return getErrors(ModelState);
            } else
            {
                _dbContext.ServiceTracker.Add(serviceEvent); //add model
                _dbContext.SaveChanges(); //save changes
                return "Success";
            }
        }

        public IActionResult DisciplinaryForm(string id) //id is resident id
        {
            ViewBag.DisciplinaryTypes = getDisciplinaryTypes(); //gets and formats DisciplinaryTypes enum as dropdown id/value pair
            ViewBag.residentId = id;
            ViewBag.currentUserName = HttpContext.Session.GetString("userName");
            ViewBag.stayId = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckOutDateTime is NULL and residentId=@query", new SqlParameter("@query", id)).ToList().First().stayId; //second verse, same as the first
            return View();
        }

        [HttpPost]
        public string SubmitDisciplinaryForm(DisciplinaryEvent disciplinaryEvent)  //when the user clicks create, calls this with ajax. 
        {
            if (!ModelState.IsValid)
            {
                return getErrors(ModelState);
            } else
            {
                _dbContext.DisciplinaryTracker.Add(disciplinaryEvent);
                _dbContext.SaveChanges();
                return "Success";
            }
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

        public IActionResult ViewServices()
        {
            return View(_dbContext.ServicesOffered.ToList());
        }

        public IActionResult CreateService()
        {
            return View();
        }

        [HttpPost]
        public string CreateService(Service newService)
        {
            if (!ModelState.IsValid)
            {
                return getErrors(ModelState);
            } else if (_dbContext.ServicesOffered.Find(newService.serviceName) != null)
            {
                return $"Service with name '{newService.serviceName}' already exists";
            } else
            {
                _dbContext.ServicesOffered.Add(newService);
                _dbContext.SaveChanges();
                return "Success";
            }
        }

        public IActionResult EditService(string serviceName)
        {
            return View(_dbContext.ServicesOffered.Find(serviceName));
        }

        [HttpPost]
        public string EditService(Service changedService)
        {
            if (!ModelState.IsValid)
            {
                return getErrors(ModelState);
            } else
            {
                Service ogService = _dbContext.ServicesOffered.Find(changedService.serviceName);
                ogService.startDate = changedService.startDate;
                ogService.endDate = changedService.endDate;
                ogService.description = changedService.description;
                _dbContext.SaveChanges();
                return "Success";
            }
        }

        [HttpDelete]
        public string DeleteService(Service deletedService)
        {
            if (_dbContext.ServicesOffered.Find(deletedService.serviceName) != null)
            {
                _dbContext.ServicesOffered.Remove(_dbContext.ServicesOffered.Find(deletedService.serviceName));
                _dbContext.SaveChanges();
                return "Success";
            }
            else return "Service does not exist";

        }

        [NonAction]
        public string getErrors(ModelStateDictionary modelState)
        {
            string errors = string.Empty;
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (ModelError error in allErrors)
            {
                errors += $"{error.ErrorMessage}\n";
            }
            return errors;
        }
    }
}
