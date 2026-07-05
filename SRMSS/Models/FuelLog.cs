using System.ComponentModel.DataAnnotations;

namespace SRMSS.Models
{
    public class FuelLog
    {
        [Key]
        public int FuelLogID { get; set; }

        public string BusNumber { get; set; }

        public string RouteCode { get; set; }

        public DateTime FuelDate { get; set; }

        public decimal FuelQuantity { get; set; }

        public decimal FuelCost { get; set; }

        public int OdometerReading { get; set; }

        public string FuelStation { get; set; }

        public string RecordedBy { get; set; }

        public string Remarks { get; set; }
    }
}