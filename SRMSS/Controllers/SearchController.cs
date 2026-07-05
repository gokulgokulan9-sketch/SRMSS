using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMSS.Data;

namespace SRMSS.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return View(new List<object>());
            }

            keyword = keyword.ToLower();

            var buses = _context.Buses
                .Where(b =>
                    b.BusNumber.ToLower().Contains(keyword) ||
                    b.BusModel.ToLower().Contains(keyword))
                .ToList();

            var routes = _context.Routes
                .Where(r =>
                    r.RouteCode.ToLower().Contains(keyword) ||
                    r.RouteName.ToLower().Contains(keyword) ||
                    r.StartPoint.ToLower().Contains(keyword) ||
                    r.EndPoint.ToLower().Contains(keyword))
                .ToList();

            var drivers = _context.Drivers
                .Where(d =>
                    d.DriverName.ToLower().Contains(keyword) ||
                    d.AssignedRoute.ToLower().Contains(keyword))
                .ToList();

            ViewBag.Buses = buses;
            ViewBag.Routes = routes;
            ViewBag.Drivers = drivers;

            return View();
        }
    }
}