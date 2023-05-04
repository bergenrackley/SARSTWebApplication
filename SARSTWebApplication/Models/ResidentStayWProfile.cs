using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class ResidentStayWProfile //this doesnt inherit from stay or resident because that messes with the dbcontext for some reason. I dont like it either.
    {
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [Display(Name = "Distinguishing Features")]
        public string distinguishingFeatures { get; set; }
        [Display(Name = "Stay Id")]
        public int stayId { get; set; }
        [Display(Name = "Resident Id")]
        public string residentId { get; set; }
        [Display(Name = "Check In Date")]
        public DateTime checkinDateTime { get; set; }
        [Display(Name = "Check Out Date")]
        public Nullable<DateTime> checkoutDateTime { get; set; }
        [Display(Name = "Sarst User Name")]
        public string userName { get; set; } // Key of sarst User

        public ResidentStayWProfile()
        {
            firstName = string.Empty;
            lastName = string.Empty;
            distinguishingFeatures = string.Empty;
            stayId = new int();
            residentId = string.Empty; // Resident
            checkinDateTime = new DateTime();
            checkoutDateTime = null;
            userName = String.Empty; // who checks them out
        }
    }
}
