
using System.ComponentModel.DataAnnotations;

namespace SRMSS.Models
{
    public class RegisterModel
    {
        public string FullName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}

