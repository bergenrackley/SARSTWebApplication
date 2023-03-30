using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class ResidentStay
    {

        [Key]
        public int stayId { get; set; }
        public string residentId { get; set; }
        [DataType(DataType.Date)]
        public DateTime checkinDateTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime checkoutDateTime { get; set; }
        public string providerUserName { get; set; }

        public ResidentStay() 
        {
            stayId = new int();
            residentId = string.Empty; // Resident
            checkinDateTime = new DateTime();
            checkoutDateTime = new DateTime();
            providerUserName = String.Empty; // who checks them out
        }
    }
}
