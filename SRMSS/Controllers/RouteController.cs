
using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;
using SRMSS.Helpers;
using SRMSS.Models;

namespace SRMSS.Controllers
{
    public class RouteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RouteController(ApplicationDbContext context)
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

            var routes = _context.Routes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                routes = routes.Where(x =>
                    x.RouteCode.Contains(search) ||
                    x.RouteName.Contains(search) ||
                    x.StartPoint.Contains(search) ||
                    x.EndPoint.Contains(search));
            }

            int totalRecords = routes.Count();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages =
                (int)Math.Ceiling((double)totalRecords / pageSize);

            var data = routes
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(RouteInfo route)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            if (ModelState.IsValid)
            {
                _context.Routes.Add(route);

                _context.Activities.Add(new Activity
                {
                    Message = $"Route {route.RouteCode} - {route.RouteName} created",
                    ActivityDate = DateTime.Now
                });

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(route);
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

            var route = _context.Routes.Find(id);

            if (route == null)
                return NotFound();

            return View(route);
        }

        // ==========================
        // EDIT POST
        // Admin + Staff
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RouteInfo route)
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff");

            if (access != null)
                return access;

            if (ModelState.IsValid)
            {
                _context.Routes.Update(route);

                _context.Activities.Add(new Activity
                {
                    Message = $"Route {route.RouteCode} updated",
                    ActivityDate = DateTime.Now
                });

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(route);
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

            var route = _context.Routes.Find(id);

            if (route == null)
                return NotFound();

            return View(route);
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

            var route = _context.Routes.Find(id);

            if (route != null)
            {
                _context.Activities.Add(new Activity
                {
                    Message = $"Route {route.RouteCode} deleted",
                    ActivityDate = DateTime.Now
                });

                _context.Routes.Remove(route);

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

