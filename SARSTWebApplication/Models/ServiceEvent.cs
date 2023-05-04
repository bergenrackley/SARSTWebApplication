using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SARSTWebApplication.Models
{
    public class ServiceEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int serviceEventId { get; set; }
        [Display(Name = "Service Name")]
        public string serviceName { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Date Provided")]
        public DateTime dateProvided { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        [Display(Name = "Resident Id")]
        public string residentId { get; set; }
        [Display(Name = "Sarst User Name")]
        public string userName { get; set; }
        [Display(Name = "Stay Id")]
        public int stayId { get; set; }

        public ServiceEvent()
        {
            serviceEventId = new int();
            serviceName = string.Empty;
            dateProvided = new DateTime();
            description = string.Empty;
            residentId = string.Empty;
            userName = string.Empty;
            stayId = new int();
        }
    }
}
