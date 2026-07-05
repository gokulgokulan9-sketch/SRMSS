namespace SRMSS.Models
{
    public class Driver
    {
        public int DriverID { get; set; }

        public string DriverName { get; set; }

        public string LicenseNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public DateOnly DateofBirth { get; set; }

        public DateOnly LicenseExpiryDate { get; set; }

        public string AssignedRoute { get; set; }

        public string WorkingHours { get; set; }

        public string Status { get; set; }

        public string Email { get; set; }
    }
}