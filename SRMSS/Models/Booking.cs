using System;
using System.ComponentModel.DataAnnotations;

namespace SRMSS.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public string RouteCode { get; set; }

        [Required]
        public string BusNumber { get; set; }

        [Required]
        public int SeatNumber { get; set; }

        [Required]
        public string PassengerName { get; set; }

        public string PassengerPhone { get; set; }

        public string PassengerEmail { get; set; }

        public DateTime BookingDate { get; set; }

        [Required]
        public DateOnly JourneyDate { get; set; }

        public decimal TicketPrice { get; set; }

        public string BookingStatus { get; set; }
    }
}