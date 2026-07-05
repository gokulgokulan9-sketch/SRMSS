
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMSS.Data;
using SRMSS.Helpers;
using SRMSS.Models;

namespace SRMSS.Controllers
{
    public class FuelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FuelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================
        // INDEX
        // Admin + Staff + Supervisor
        // ======================
        public async Task<IActionResult> Index(string searchString)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff",
                "Supervisor");

            if (access != null)
                return access;

            var fuelLogs = from f in _context.FuelLogs
                           select f;

            if (!string.IsNullOrEmpty(searchString))
            {
                fuelLogs = fuelLogs.Where(f =>
                    f.BusNumber.Contains(searchString) ||
                    f.RouteCode.Contains(searchString) ||
                    f.RecordedBy.Contains(searchString));
            }

            return View(await fuelLogs.ToListAsync());
        }

        // ======================
        // CREATE GET
        // Admin + Staff
        // ======================
        public IActionResult Create()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            return View();
        }

        // ======================
        // CREATE POST
        // Admin + Staff
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FuelLog fuelLog)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            if (ModelState.IsValid)
            {
                _context.FuelLogs.Add(fuelLog);

                _context.Activities.Add(new Activity
                {
                    Message = $"Fuel record added for Bus {fuelLog.BusNumber}",
                    ActivityDate = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }

            return View(fuelLog);
        }

        // ======================
        // EDIT GET
        // Admin + Staff
        // ======================
        public async Task<IActionResult> Edit(int? id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            if (id == null)
                return NotFound();

            var fuelLog = await _context.FuelLogs.FindAsync(id);

            if (fuelLog == null)
                return NotFound();

            return View(fuelLog);
        }

        // ======================
        // EDIT POST
        // Admin + Staff
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FuelLog fuelLog)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            if (id != fuelLog.FuelLogID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fuelLog);

                    _context.Activities.Add(new Activity
                    {
                        Message = $"Fuel record updated for Bus {fuelLog.BusNumber}",
                        ActivityDate = DateTime.Now
                    });

                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(fuelLog);
        }

        // ======================
        // DELETE GET
        // Admin only
        // ======================
        public async Task<IActionResult> Delete(int? id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin");

            if (access != null)
                return access;

            if (id == null)
                return NotFound();

            var fuelLog = await _context.FuelLogs
                .FirstOrDefaultAsync(m => m.FuelLogID == id);

            if (fuelLog == null)
                return NotFound();

            return View(fuelLog);
        }

        // ======================
        // DELETE POST
        // Admin only
        // ======================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin");

            if (access != null)
                return access;

            var fuelLog = await _context.FuelLogs.FindAsync(id);

            if (fuelLog != null)
            {
                _context.Activities.Add(new Activity
                {
                    Message = $"Fuel record deleted for Bus {fuelLog.BusNumber}",
                    ActivityDate = DateTime.Now
                });

                _context.FuelLogs.Remove(fuelLog);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

