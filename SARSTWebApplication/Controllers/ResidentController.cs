using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;

namespace SARSTWebApplication.Controllers
{
    public class ResidentController : Controller
    {

        private AppDbContext _dbContext;

        public ResidentController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
        }

        //-------Pages--------// Get Pages

        // GET: /Residen
        public ActionResult Index()
        {

            var residents = _dbContext.Residents.ToList();
            ViewBag.Residents = residents;
            return View();
        }

        // GET: /Resident/Register
        // Submit Registration Request
        public IActionResult Register()
        {
            var model = new Resident();
            return View("Register", model);
        }

        public ActionResult Success()
        {
            return View("Success");
        }

        //-------Methods--------// Post Pages

        // POST: ResidentController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Resident model)
        {
            try
            {
                Resident existingResident = _dbContext.Residents.Find(model.residentId);

                if (existingResident == null)
                {
                    return AddRequest(model); ;
                }
                else
                {
                    TempData["Message"] = "ID already Exist";
                    return View();
                }

            }
            catch
            {
                return View();
            }
        }

        // AddUser
        // Writes the ResidentProfile to the database table and loads the Success view
        public IActionResult AddRequest(Resident newRequest)
        {

            Resident newRow = newRequest;
            try
            {
                _dbContext.Residents.Add(newRow);
                _dbContext.SaveChanges();

                var resident = _dbContext.Residents.FirstOrDefault(r => r.residentId == newRow.residentId);
                if (resident != null)
                {
                    Console.WriteLine("The row was added successfully");

                }
                else { Console.WriteLine("The row was not added"); }

                return Success();

            }
            catch (DbUpdateException ex)
            { // Get the inner exception
                var innerException = ex.InnerException; // Print out the message and stack trace
                Console.WriteLine(innerException.Message);
                Console.WriteLine(innerException.StackTrace); // Handle the exception or rethrow it }
                return Success();

            }
        }

    }
}
