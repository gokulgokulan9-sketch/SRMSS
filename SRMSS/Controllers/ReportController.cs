
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using SRMSS.Data;
using SRMSS.Helpers;
using SRMSS.Models;

namespace SRMSS.Controllers
{
    public partial class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // REPORT DASHBOARD
        // Admin + Supervisor
        // =========================
        public IActionResult Index()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            ViewBag.TotalBuses = _context.Buses.Count();
            ViewBag.TotalDrivers = _context.Drivers.Count();
            ViewBag.TotalRoutes = _context.Routes.Count();
            ViewBag.TotalSchedules = _context.Schedules.Count();
            ViewBag.TotalFuelLogs = _context.FuelLogs.Count();
            ViewBag.TotalMaintenanceLogs = _context.MaintenanceLogs.Count();
            ViewBag.TotalUsers = _context.Users.Count();

            ViewBag.TotalFuelCost =
                _context.FuelLogs.Sum(x => x.FuelCost);

            ViewBag.TotalMaintenanceCost =
                _context.MaintenanceLogs.Sum(x => x.ServiceCost);

            return View();
        }

        // =========================
        // BUS REPORT
        // =========================
        public IActionResult BusReport()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var buses = _context.Buses.ToList();

            return View(buses);
        }

        // =========================
        // BUS REPORT PDF
        // =========================
        public IActionResult BusReportPDF()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var buses = _context.Buses.ToList();

            return new ViewAsPdf("BusReport", buses)

            {
                FileName = "BusReport.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
           
        }

        // =========================
        // DRIVER REPORT
        // =========================
        public IActionResult DriverReport()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var drivers = _context.Drivers.ToList();

            return View(drivers);
        }

        // =========================
        // DRIVER REPORT PDF
        // =========================
        public IActionResult DriverReportPDF()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var drivers = _context.Drivers.ToList();

            return new ViewAsPdf("DriverReport", drivers)
            {
                FileName = "Driver_Report.pdf",
                PageOrientation =
                 Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
        // =========================
        // ROUTE REPORT
        // =========================
        public IActionResult RouteReport()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var routes = _context.Routes.ToList();

            return View(routes);
        }

        // =========================
        // ROUTE REPORT PDF
        // =========================
        public IActionResult RouteReportPDF()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var routes = _context.Routes.ToList();

            return new ViewAsPdf("RouteReport", routes)
            {
                FileName = "Route_Report.pdf",
                PageOrientation =
                Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        // =========================
        // SCHEDULE REPORT
        // =========================
        public IActionResult ScheduleReport()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var schedules = _context.Schedules.ToList();

            return View(schedules);
        }

        // =========================
        // SCHEDULE REPORT PDF
        // =========================
        public IActionResult ScheduleReportPDF()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var schedules = _context.Schedules.ToList();

            return new ViewAsPdf("ScheduleReport", schedules)
            {
                FileName = "Schedule_Report.pdf",
                PageOrientation =
                Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        // =========================
        // FUEL REPORT
        // =========================
        public IActionResult FuelReport()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var fuels = _context.FuelLogs.ToList();

            return View(fuels);
        }

        // =========================
        // FUEL REPORT PDF
        // =========================
        public IActionResult FuelReportPDF()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var fuels = _context.FuelLogs.ToList();

            return new ViewAsPdf("FuelReport", fuels)
            {
                FileName = "Fuel_Report.pdf",
                PageOrientation =
                Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        // =========================
        // MAINTENANCE REPORT
        // =========================
        public IActionResult MaintenanceReport()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var maintenance = _context.MaintenanceLogs.ToList();

            return View(maintenance);
        }

        // =========================
        // MAINTENANCE REPORT PDF
        // =========================
        public IActionResult MaintenanceReportPDF()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var maintenance = _context.MaintenanceLogs.ToList();

            return new ViewAsPdf("MaintenanceReport", maintenance)
            {
                FileName = "Maintenance_Report.pdf",
                PageOrientation =
                Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        // =========================
        // USER REPORT
        // =========================
        public IActionResult UserReport()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var users = _context.Users.ToList();

            return View(users);
        }

        // =========================
        // USER REPORT PDF
        // =========================
        public IActionResult UserReportPDF()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var users = _context.Users.ToList();

            return new ViewAsPdf("UserReport", users)
            {
                FileName = "User_Report.pdf",
                PageOrientation =
                Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        // =========================
        // ALL REPORTS
        // =========================
        public IActionResult AllReports()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var model = new AllReportsViewModel
            {
                Buses = _context.Buses.ToList(),
                Drivers = _context.Drivers.ToList(),
                Routes = _context.Routes.ToList(),
                Schedules = _context.Schedules.ToList(),
                FuelLogs = _context.FuelLogs.ToList(),
                MaintenanceLogs = _context.MaintenanceLogs.ToList(),
                Users = _context.Users.ToList()
            };

            return View(model);
        }

        // =========================
        // ALL REPORTS PDF
        // =========================
        public IActionResult AllReportsPDF()
        {
            var access = RoleHelper.CheckRole(
                HttpContext,
                "Admin",
                "Supervisor");

            if (access != null)
                return access;

            var model = new AllReportsViewModel
            {
                Buses = _context.Buses.ToList(),
                Drivers = _context.Drivers.ToList(),
                Routes = _context.Routes.ToList(),
                Schedules = _context.Schedules.ToList(),
                FuelLogs = _context.FuelLogs.ToList(),
                MaintenanceLogs = _context.MaintenanceLogs.ToList(),
                Users = _context.Users.ToList()
            };

            return new ViewAsPdf("AllReports", model)
            {
                FileName = "All_Reports.pdf",
                PageOrientation =
                Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}



