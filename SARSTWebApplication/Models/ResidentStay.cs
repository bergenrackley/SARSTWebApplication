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
        [DataType(DataType.Date)]
        public DateTime checkinDateTime { get; set; }
        [DataType(DataType.Date)]
        public Nullable<DateTime> checkoutDateTime { get; set; }
        public string userName { get; set; } // Key of sarst User

        public ResidentStay()
        {
            stayId = new int();
            residentId = string.Empty; // Resident
            checkinDateTime = new DateTime();
            checkoutDateTime = null;
            userName = String.Empty; // who checks them out
        }
    }
}
