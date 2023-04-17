using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;
using System.Data;

namespace SARSTWebApplication.Controllers
{
    public class ServiceTrackerController : Controller
    {
        private AppDbContext _dbContext;
        public ServiceTrackerController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
        }

        public IActionResult Index()
        {
            ViewBag.ServiceList = ServicesToList(_dbContext.ServicesOffered.ToList());
            return View();
        }

        public IActionResult SelectResident() {
            return View(_dbContext.Residents.ToList());
        }

        public IActionResult SubmitForm(string id)
        {
            ViewBag.ServiceList = ServicesToList(_dbContext.ServicesOffered.ToList());
            ViewBag.residentId = id;
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


    }
}
