using System;
using System.ComponentModel.DataAnnotations;

namespace SRMSS.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }

        [Required]
        public string RouteCode { get; set; }

        [Required]
        public string BusNumber { get; set; }

        [Required]
        public string DriverName { get; set; }

        [Required]
        public DateTime ScheduleDate { get; set; }

        [Required]
        public TimeSpan DepartureTime { get; set; }

        [Required]
        public TimeSpan ArrivalTime { get; set; }

        [Required]
        public string TripType { get; set; }

        [Required]
        public string TripStatus { get; set; }

        public string Remarks { get; set; }
    }
}