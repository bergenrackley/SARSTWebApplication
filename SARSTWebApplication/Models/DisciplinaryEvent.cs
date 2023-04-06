using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace SARSTWebApplication.Models
{
    public enum disciplinaryActionType
    {
        Warning,
        Education,
        LastChanceContact,
        Step_Away
    }

    public class DisciplinaryEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int disciplinaryEventId { get; set; }

        [ForeignKey("ResidentStay")]
        public int residentStayId { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime dateIssued { get; set; }

        public disciplinaryActionType typeOfDisciplinaryAction { get; set; }

        public string reasonGiven { get; set; }
        public DisciplinaryEvent()
        {
            typeOfDisciplinaryAction = disciplinaryActionType.Warning;
            disciplinaryEventId = 0;
            residentStayId = 0;
            dateIssued = DateTime.MinValue;
        }
    }
}
