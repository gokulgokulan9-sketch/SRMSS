
using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;
using SRMSS.Helpers;
using SRMSS.Models;

namespace SRMSS.Controllers
{
    public class DriverController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriverController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // INDEX
        // Admin + Staff + Supervisor
        // ==========================
        public IActionResult Index(string search, int page = 1)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff",
                "Supervisor");

            if (access != null)
                return access;

            int pageSize = 10;

            var drivers = _context.Drivers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                drivers = drivers.Where(x =>
                    x.DriverName.Contains(search) ||
                    x.LicenseNumber.Contains(search) ||
                    x.AssignedRoute.Contains(search));
            }

            int totalRecords = drivers.Count();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages =
                (int)Math.Ceiling((double)totalRecords / pageSize);

            var data = drivers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(data);
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
        public IActionResult Create(Driver driver)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            _context.Drivers.Add(driver);

            _context.Activities.Add(new Activity
            {
                Message = $"Driver {driver.DriverName} added successfully",
                ActivityDate = DateTime.Now
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
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

            var driver = _context.Drivers.Find(id);

            if (driver == null)
                return NotFound();

            return View(driver);
        }

        // ==========================
        // EDIT POST
        // Admin + Staff
        // ==========================
        [HttpPost]
        public IActionResult Edit(Driver driver)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            _context.Drivers.Update(driver);

            _context.Activities.Add(new Activity
            {
                Message = $"Driver {driver.DriverName} updated",
                ActivityDate = DateTime.Now
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ==========================
        // DELETE PAGE
        // Admin only
        // ==========================
        public IActionResult Delete(int id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin");

            if (access != null)
                return access;

            var driver = _context.Drivers.Find(id);

            if (driver == null)
                return NotFound();

            return View(driver);
        }

        // ==========================
        // DELETE POST
        // Admin only
        // ==========================
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin");

            if (access != null)
                return access;

            var driver = _context.Drivers.Find(id);

            if (driver != null)
            {
                _context.Activities.Add(new Activity
                {
                    Message = $"Driver {driver.DriverName} deleted",
                    ActivityDate = DateTime.Now
                });

                _context.Drivers.Remove(driver);

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

