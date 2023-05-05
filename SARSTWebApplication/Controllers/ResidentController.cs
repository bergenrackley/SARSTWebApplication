using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using SARSTWebApplication.Data;
using SARSTWebApplication.Enums;
using SARSTWebApplication.Models;
using System.Data.SqlClient;
using System.Web.WebPages.Html;

namespace SARSTWebApplication.Controllers
{
    public class ResidentController : BaseController
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
        public IActionResult Create()
        {
            ViewBag.residentSex = getSex();
            ViewBag.residentGender = getGender();
            ViewBag.residentPronouns = getPronouns();

            return View(new Resident());
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
        public string CreateResident(Resident model)
        {
            int numRes;
            if (_dbContext.Residents.ToList().Count() == 0)
            {
                numRes = 1;
            }
            else
            {
                numRes = Int32.Parse(_dbContext.Database.SqlQuery<Resident>("Select * from [dbo].[Residents] order by residentId desc").ToList().First().residentId.Split("0").Last()) + 1;
            }
            model.residentId = "SAResidentID" + String.Format("{0:000000000}", numRes);

            if (!ModelState.IsValid)
            {
                string errors = string.Empty;
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (ModelError error in allErrors)
                {
                    errors += $"{error.ErrorMessage}\n";
                }
                return errors;
            } else
            {
                _dbContext.Residents.Add(model);
                _dbContext.SaveChanges();
                return "Success";
            }
        }

        // GET: ResidentStays/Edit/5
        public IActionResult Edit(string id)
        {
            ViewBag.residentSex = getSex();
            ViewBag.residentGender = getGender();
            ViewBag.residentPronouns = getPronouns();
            ViewBag.status = getDisciplinary();
            var resident = _dbContext.Residents.Find(id);
            return View(resident);
        }

        // POST: ResidentStays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public string EditResident(Resident resident)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Empty;
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (ModelError error in allErrors)
                {
                    errors += $"{error.ErrorMessage}\n";
                }
                return errors;
            }
            // Find the existing entity by its primary key
            var existingResident = _dbContext.Residents.Find(resident.residentId);

            // Assign the new values to its properties
            existingResident.firstName = resident.firstName;
            existingResident.lastName = resident.lastName;
            existingResident.dateOfBirth = resident.dateOfBirth;
            existingResident.sex = resident.sex;
            existingResident.gender = resident.gender;
            existingResident.pronouns = resident.pronouns;
            existingResident.distinguishingFeatures = resident.distinguishingFeatures;
            existingResident.status = resident.status;

            // Save the changes
            _dbContext.SaveChanges();
            return "Success";
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

        [HttpGet]
        public PartialViewResult SearchResidents(string query)
        { //this partial view gets called by ajax, creates teh table seen in selectresident. does the searching and refreshes the partialview on keyup event from seach box
            List<Resident> result = _dbContext.Database.SqlQuery<Resident>("Select * from dbo.residents where firstName + ' ' + lastName like @query or distinguishingFeatures like @query", new SqlParameter("@query", "%" + query + "%")).ToList();
            return PartialView("_GridResidents", result);
        }

        [NonAction]
        public List<SelectListItem> getSex()
        {
            return Enum.GetValues(typeof(ResidentSex)).Cast<ResidentSex>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
        }

        [NonAction]
        public List<SelectListItem> getGender()
        {
            return Enum.GetValues(typeof(ResidentGender)).Cast<ResidentGender>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
        }

        [NonAction]
        public List<SelectListItem> getPronouns()
        {
            return Enum.GetValues(typeof(ResidentPronouns)).Cast<ResidentPronouns>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
        }

        [NonAction]
        public List<SelectListItem> getDisciplinary()
        {
            return Enum.GetValues(typeof(DisciplinaryTypes)).Cast<DisciplinaryTypes>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
        }

    }
}
