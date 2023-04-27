namespace SARSTWebApplication.Models
{
    public class ResidentStayWProfile //this doesnt inherit from stay or resident because that messes with the dbcontext for some reason. I dont like it either.
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
