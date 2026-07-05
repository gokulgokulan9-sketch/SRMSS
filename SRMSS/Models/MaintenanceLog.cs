using System;
using System.ComponentModel.DataAnnotations;

namespace SRMSS.Models
{
    public class MaintenanceLog
    {
        [Key]
        public int MaintenanceID { get; set; }

        public string BusNumber { get; set; }

        public DateTime MaintenanceDate { get; set; }

        public string MaintenanceType { get; set; }

        public string Description { get; set; }

        public decimal ServiceCost { get; set; }

        public DateTime NextServiceDate { get; set; }

        public string MechanicName { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }
    }
}