using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SARSTWebApplication.Models
{
    public class ResidentStay
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int stayId { get; set; }
        public string residentId { get; set; } // key of resident
        public DateTime checkinDateTime { get; set; }
        public DateTime? checkoutDateTime { get; set; }
        public string userName { get; set; } // Key of sarst User

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
