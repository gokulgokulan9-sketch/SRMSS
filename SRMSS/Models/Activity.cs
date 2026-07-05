
namespace SRMSS.Models
{
    public class Activity
    {
        public int ActivityID { get; set; }

        public string Message { get; set; } = "";

        public DateTime ActivityDate { get; set; }

        public bool IsRead { get; set; } = false;
    }
}

