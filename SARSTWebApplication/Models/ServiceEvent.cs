using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SARSTWebApplication.Models
{
    public class ServiceEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int serviceEventId { get; set; }
        public string serviceName { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime dateProvided { get; set; }
        public string description { get; set; }
        public string residentId { get; set; }
        public string userName { get; set; }
        public int stayId { get; set; }

        public ServiceEvent() {
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
