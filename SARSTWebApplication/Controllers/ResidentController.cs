using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;
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

        // GET: /Resident
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

        // [Route("resident/Details/{id}")]
        public IActionResult Details(int id)
        {
            Resident res = _dbContext.Residents.Find(id.ToString());
            return View(res);
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


        // GET: ResidentStays/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            var resident = _dbContext.Residents.Find(id);

            if (resident == null)
                return NotFound();

            return View(resident);
        }

        // POST: ResidentStays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("residentId,firstName,lastName,dateOfBirth,sex, gender, pronouns, distinguishingFeatures, status")] Resident resident)
        {
            if (id != resident.residentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing entity by its primary key
                    var existingResident = _dbContext.Residents.Find(id);

                    // Assign the new values to its properties
                    existingResident.residentId = resident.residentId;
                    existingResident.firstName = resident.firstName;
                    existingResident.lastName = resident.lastName;
                    existingResident.sex = resident.sex;
                    existingResident.gender = resident.gender;
                    existingResident.pronouns = resident.pronouns;
                    existingResident.distinguishingFeatures = resident.distinguishingFeatures;
                    existingResident.status = resident.status;

                    // Save the changes
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_dbContext.ResidentStays.Find(resident.residentId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(resident);
        }

        // GET: ResidentStays/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            var resident = _dbContext.Residents.Find(id);

            if (resident == null)
                return NotFound();

            return View(resident);
        }

        // POST: ResidentStays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirme(string id)
        {

            var resident = _dbContext.Residents.Find(id);

            if (resident == null)
            {
                return Problem("Entity set 'SARSTWebApplicationContext.Resident'  is null.");
            }
            else
                _dbContext.Residents.Remove(resident);


            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
