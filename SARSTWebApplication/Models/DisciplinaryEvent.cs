using SARSTWebApplication.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SARSTWebApplication.Models
{
    public class DisciplinaryEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int disciplinaryEventId { get; set; }
        public DisciplinaryTypes? disciplinaryType { get; set; }

        [DataType(DataType.Date)]
        public DateTime dateProvided { get; set; }
        public string description { get; set; }
        public string residentId { get; set; }
        public string userName { get; set; }
        public int stayId { get; set; }

        public DisciplinaryEvent() {
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
