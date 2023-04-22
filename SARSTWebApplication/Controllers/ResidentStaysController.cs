using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;

namespace SARSTWebApplication.Controllers
{
    public class ResidentStaysController : Controller
    {
        private AppDbContext _dbContext;

        public ResidentStaysController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
        }

        // GET: ResidentStays
        public async Task<IActionResult> Index()
        {
            var residentStays = _dbContext.ResidentStays.ToList();
            ViewBag.ResidentStays = residentStays;
            return View();
        }

        // GET: ResidentStays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var residentStay = _dbContext.ResidentStays.Find(id);

            if (residentStay == null)
                return NotFound();

            return View(residentStay);
        }

        // GET: ResidentStays/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: ResidentStays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("stayId,residentId,checkinDateTime,checkoutDateTime,userName")] ResidentStay residentStay)
        {
            if (ModelState.IsValid)
            {
                _dbContext.ResidentStays.Add(residentStay);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(residentStay);
        }

        // GET: ResidentStays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var residentStay = _dbContext.ResidentStays.Find(id);

            if (residentStay == null)
                return NotFound();

            return View(residentStay);
        }

        // POST: ResidentStays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("stayId,residentId,checkinDateTime,checkoutDateTime,userName")] ResidentStay residentStay)
        {
            if (id != residentStay.stayId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing entity by its primary key
                    var existingResidentStay = _dbContext.ResidentStays.Find(id);

                    // Assign the new values to its properties
                    existingResidentStay.residentId = residentStay.residentId;
                    existingResidentStay.checkinDateTime = residentStay.checkinDateTime;
                    existingResidentStay.checkoutDateTime = residentStay.checkoutDateTime;
                    existingResidentStay.userName = residentStay.userName;

                    // Save the changes
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_dbContext.ResidentStays.Find(residentStay.stayId) == null)
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
            return View(residentStay);
        }

        // GET: ResidentStays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var residentStay = _dbContext.ResidentStays.Find(id);

            if (residentStay == null)
                return NotFound();

            return View(residentStay);
        }

        // POST: ResidentStays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var residentStay = _dbContext.ResidentStays.Find(id);

            if (residentStay == null)
            {
                return Problem("Entity set 'SARSTWebApplicationContext.ResidentStay'  is null.");
            }
            else
                _dbContext.ResidentStays.Remove(residentStay);


            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
