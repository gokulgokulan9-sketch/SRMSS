
using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;
using SRMSS.Helpers;
using SRMSS.Models;

namespace SRMSS.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // INDEX
        // Admin + Staff + Supervisor
        // ==========================
        public IActionResult Index(string search)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff",
                "Supervisor");

            if (access != null)
                return access;

            var maintenance = _context.MaintenanceLogs.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                maintenance = maintenance.Where(x =>
                    x.BusNumber.Contains(search) ||
                    x.MechanicName.Contains(search) ||
                    x.MaintenanceType.Contains(search));
            }

            return View(maintenance.ToList());
        }

        // ==========================
        // CREATE GET
        // Admin + Staff
        // ==========================
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

        // ==========================
        // CREATE POST
        // Admin + Staff
        // ==========================
        [HttpPost]
        public IActionResult Create(MaintenanceLog maintenance)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            _context.MaintenanceLogs.Add(maintenance);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // EDIT GET
        // Admin + Staff
        // ==========================
        public IActionResult Edit(int id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            var maintenance = _context.MaintenanceLogs.Find(id);

            if (maintenance == null)
                return NotFound();

            return View(maintenance);
        }

        // ==========================
        // EDIT POST
        // Admin + Staff
        // ==========================
        [HttpPost]
        public IActionResult Edit(MaintenanceLog maintenance)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            _context.MaintenanceLogs.Update(maintenance);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // DELETE GET
        // Admin only
        // ==========================
        public IActionResult Delete(int id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin");

            if (access != null)
                return access;

            var maintenance = _context.MaintenanceLogs.Find(id);

            if (maintenance == null)
                return NotFound();

            return View(maintenance);
        }

        // ==========================
        // DELETE POST
        // Admin only
        // ==========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin");

            if (access != null)
                return access;

            var maintenance = _context.MaintenanceLogs.Find(id);

            if (maintenance != null)
            {
                _context.MaintenanceLogs.Remove(maintenance);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

