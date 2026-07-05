
using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;
using SRMSS.Helpers;

namespace SRMSS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

     
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // DASHBOARD
        // Admin + Staff + Supervisor + User
        // ==========================
        public IActionResult Index()
        {
            ViewBag.TotalBookings = _context.Bookings.Count();

            ViewBag.TodayBookings = _context.Bookings
                .Count(x => x.BookingDate.Date == DateTime.Today);

            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Staff",
                "Supervisor",
                "User");

            if (access != null)
                return access;

            // ==========================
            // BUS
            // ==========================
            ViewBag.TotalBuses = _context.Buses.Count();
            ViewBag.Available =
                _context.Buses.Count(x => x.AvailabilityStatus == "Available");

            ViewBag.Assigned =
                _context.Buses.Count(x => x.AvailabilityStatus == "Assigned");

            ViewBag.Maintenance =
                _context.Buses.Count(x => x.AvailabilityStatus == "Maintenance");

            // ==========================
            // DRIVER
            // ==========================
            ViewBag.TotalDrivers = _context.Drivers.Count();

            ViewBag.AssignedDrivers =
                _context.Drivers.Count(x => x.Status == "Assigned");

            ViewBag.AvailableDriversCount =
                _context.Drivers.Count(x => x.Status == "Available");

            ViewBag.OnLeaveDrivers =
                _context.Drivers.Count(x => x.Status == "On Leave");

            // ==========================
            // ROUTES
            // ==========================
            ViewBag.TotalRoutes = _context.Routes.Count();

            ViewBag.ActiveRoutes =
                _context.Routes.Count(x => x.RouteStatus == "Active");

            ViewBag.InactiveRoutes =
                _context.Routes.Count(x => x.RouteStatus == "Inactive");

            // ==========================
            // SCHEDULE
            // ==========================
            ViewBag.TotalSchedules = _context.Schedules.Count();

            // ==========================
            // FUEL
            // ==========================
            ViewBag.TotalFuelLogs = _context.FuelLogs.Count();

            ViewBag.TotalFuelCost =
                _context.FuelLogs.Sum(x => x.FuelCost);

            // ==========================
            // MAINTENANCE
            // ==========================
            ViewBag.TotalMaintenanceLogs =
                _context.MaintenanceLogs.Count();

            ViewBag.TotalMaintenanceCost =
                _context.MaintenanceLogs.Sum(x => x.ServiceCost);

            // ==========================
            // USERS
            // ==========================
            ViewBag.TotalUsers = _context.Users.Count();


            ViewBag.RecentActivities = _context.Activities
                                  .OrderByDescending(x => x.ActivityDate)
                                  .Take(5)
                                  .ToList();


            ViewBag.RecentActivities = _context.Activities
                    .OrderByDescending(x => x.ActivityDate)
                    .Take(5)
                    .ToList();

            ViewBag.UnreadCount = _context.Activities.Count(x => !x.IsRead);



            return View();

           


        }
    }
}

