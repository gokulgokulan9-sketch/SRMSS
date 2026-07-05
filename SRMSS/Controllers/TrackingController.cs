using Microsoft.AspNetCore.Mvc;
using SRMSS.Data;

namespace SRMSS.Controllers
{
    public class TrackingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrackingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Map Page
        public IActionResult Index()
        {
            return View();
        }

        // Get Vehicle Locations
        public JsonResult GetVehicleLocations()
        {
            var vehicles = (from v in _context.Buses
                            join r in _context.Routes
                            on v.BusNumber equals r.AssignedBus into routeData
                            from r in routeData.DefaultIfEmpty()
                            select new
                            {
                                v.VehicleID,
                                v.BusNumber,
                                v.BusModel,
                                DriverName = r != null ? r.AssignedDriver : "No Driver",
                                RouteName = r != null ? r.RouteName : "No Route",
                                RouteCode = r != null ? r.RouteCode : "No Route",
                                v.AvailabilityStatus,
                                v.Latitude,
                                v.Longitude
                            }).ToList();

            return Json(vehicles);
        }
    }
}