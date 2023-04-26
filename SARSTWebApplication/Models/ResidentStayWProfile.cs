using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class ResidentStayWProfile
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string distinguishingFeatures { get; set; }
        public int stayId { get; set; }
        public string residentId { get; set; }
        public DateTime checkinDateTime { get; set; }
        public Nullable<DateTime> checkoutDateTime { get; set; }
        public string userName { get; set; } // Key of sarst User

        public ResidentStayWProfile()
        {
            firstName= string.Empty;
            lastName= string.Empty;
            distinguishingFeatures= string.Empty;
            stayId = new int();
            residentId = string.Empty; // Resident
            checkinDateTime = new DateTime();
            checkoutDateTime = null;
            userName = String.Empty; // who checks them out
        }
    }
}
