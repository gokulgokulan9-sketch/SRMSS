using System.ComponentModel.DataAnnotations;

namespace SRMSS.Models
{
    public class RouteInfo
    {
        [Key]
        public int RouteID { get; set; }

        public string RouteCode { get; set; }

        public string RouteName { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public string Stops { get; set; }

        public decimal TotalDistance { get; set; }

        public string AssignedBus { get; set; }

        public string AssignedDriver { get; set; }

        public string RouteStatus { get; set; }

        public TimeSpan DepartureTime { get; set; }

        public TimeSpan ArrivalTime { get; set; }

        public int TotalSeats { get; set; }

        public decimal TicketPrice { get; set; }
    }
}