using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SARSTWebApplication.Models
{
    public class ServiceEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int serviceEventId { get; set; }
        [ForeignKey("Service")]
        public string serviceName { get; set; }

        [DataType(DataType.Date)]
        public DateTime dateProvided { get; set; }
        public string description { get; set; }
        [ForeignKey("ResidentProfile")]
        public string residentId { get; set; }
        [ForeignKey("SarstUser")]
        public string userName { get; set; }
        [ForeignKey("ResidentStay")]
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
