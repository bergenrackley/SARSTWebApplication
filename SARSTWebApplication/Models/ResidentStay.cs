using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SARSTWebApplication.Models
{
    public class ResidentStay
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Stay Id")]
        public int stayId { get; set; }
        [Display(Name = "Resident Id")]
        public string residentId { get; set; } // key of resident
        [DataType(DataType.Date)]
        [Display(Name = "Check In Date")]
        public DateTime checkinDateTime { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Check Out Date")]
        public Nullable<DateTime> checkoutDateTime { get; set; }
        [Display(Name = "Noteworthy Events")]
        public string? NoteworthyEvents { get; set; }
        [Display(Name = "User Name")]
        public string userName { get; set; } // Key of sarst User

        public ResidentStay()
        {
            stayId = new int();
            residentId = string.Empty; // Resident
            checkinDateTime = new DateTime();
            checkoutDateTime = null;
            NoteworthyEvents = null;
            userName = String.Empty; // who checks them out
        }
    }
}
