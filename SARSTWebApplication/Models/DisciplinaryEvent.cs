using SARSTWebApplication.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SARSTWebApplication.Models
{
    public class DisciplinaryEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Disciplinary Event Id")]
        public int disciplinaryEventId { get; set; }
        [Display(Name = "Disciplinary Event Type")]
        public DisciplinaryTypes? disciplinaryType { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Occurance")]
        public DateTime dateProvided { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        [Display(Name = "Resident Id")]
        public string residentId { get; set; }
        [Display(Name = "Sarst User Name")]
        public string userName { get; set; }
        [Display(Name = "Stay Id")]
        public int stayId { get; set; }

        public DisciplinaryEvent()
        {
            disciplinaryEventId = new int();
            disciplinaryType = null;
            dateProvided = new DateTime();
            description = string.Empty;
            residentId = string.Empty;
            userName = string.Empty;
            stayId = new int();
        }
    }
}
