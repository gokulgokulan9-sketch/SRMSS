using Microsoft.EntityFrameworkCore;
using SRMSS.Models;

namespace SRMSS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bus> Buses { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<RouteInfo> Routes { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<FuelLog> FuelLogs { get; set; }

        public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<SystemSetting> SystemSettings { get; set; }
    }
}