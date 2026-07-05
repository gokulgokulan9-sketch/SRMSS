using System.ComponentModel.DataAnnotations;

namespace SRMSS.Models
{
    public class SystemSetting
    {
        [Key]
        public int SettingID { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Website { get; set; }

        public string Logo { get; set; }
    }
}