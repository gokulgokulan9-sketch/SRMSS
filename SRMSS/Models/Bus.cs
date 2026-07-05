using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRMSS.Models
{
    [Table("Vehicles")]
    public class Bus
    {
        [Key]
        public int VehicleID { get; set; }

        public string BusNumber { get; set; }

        public string BusModel { get; set; }

        public string BusType { get; set; }

        public int SeatingCapacity { get; set; }

        public int Mileage { get; set; }

        public string FuelType { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string MaintenanceStatus { get; set; }

        public string AvailabilityStatus { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}