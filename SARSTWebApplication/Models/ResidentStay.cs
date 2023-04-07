using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SARSTWebApplication.Models
{
    public class ResidentStay
    {

        [Key]
        public int stayId { get; set; }
        [ForeignKey("ResidentProfile")]
        public string residentId { get; set; }
        public DateTime checkinDateTime { get; set; }
        public DateTime? checkoutDateTime { get; set; }
        [ForeignKey("SarstUser")]
        public string userName { get; set; }

        public ResidentStay() 
        {
            stayId = new int();
            residentId = string.Empty; // Resident
            checkinDateTime = new DateTime();
            checkoutDateTime = new DateTime();
            userName = String.Empty; // who checks them out
        }
    }
}
