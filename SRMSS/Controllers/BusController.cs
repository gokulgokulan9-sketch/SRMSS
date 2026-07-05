using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;
using SRMSS.Helpers;
using SRMSS.Models;

namespace SRMSS.Controllers
{
    public class BusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // INDEX (Admin + Staff + Supervisor)
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

            var buses = _context.Buses.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                buses = buses.Where(x =>
                    x.BusNumber.Contains(search) ||
                    x.BusModel.Contains(search) ||
                    x.BusType.Contains(search));
            }

            int totalRecords = buses.Count();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages =
                (int)Math.Ceiling((double)totalRecords / pageSize);

            var data = buses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(data);
        }

        // ==========================
        // CREATE GET (Admin + Staff)
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
        // CREATE POST (Admin + Staff)
        // ==========================
        [HttpPost]
        public IActionResult Create(Bus bus)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            _context.Buses.Add(bus);

            _context.Activities.Add(new Activity
            {
                Message = $"Bus {bus.BusNumber} added successfully",
                ActivityDate = DateTime.Now
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ==========================
        // EDIT GET (Admin + Staff)
        // ==========================
        public IActionResult Edit(int id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            var bus = _context.Buses.Find(id);

            if (bus == null)
            {
                return NotFound();
            }

            return View(bus);
        }

        // ==========================
        // EDIT POST (Admin + Staff)
        // ==========================
        [HttpPost]
        public IActionResult Edit(Bus bus)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            _context.Buses.Update(bus);

            _context.Activities.Add(new Activity
            {
                Message = $"Bus {bus.BusNumber} updated",
                ActivityDate = DateTime.Now
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ==========================
        // DELETE (Admin only)
        // ==========================
        public IActionResult Delete(int id)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin");

            if (access != null)
                return access;

            var bus = _context.Buses.Find(id);

            if (bus == null)
            {
                return NotFound();
            }

            _context.Activities.Add(new Activity
            {
                Message = $"Bus {bus.BusNumber} deleted",
                ActivityDate = DateTime.Now
            });

            _context.Buses.Remove(bus);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}