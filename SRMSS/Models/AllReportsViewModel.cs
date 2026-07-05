using System.Collections.Generic;

namespace SRMSS.Models
{
    public class AllReportsViewModel
    {
        public List<Bus> Buses { get; set; }
        public List<Driver> Drivers { get; set; }
        public List<RouteInfo> Routes { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<FuelLog> FuelLogs { get; set; }
        public List<MaintenanceLog> MaintenanceLogs { get; set; }
        public List<User> Users { get; set; }
    }
}