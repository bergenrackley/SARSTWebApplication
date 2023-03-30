using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class Service
    {
        [Key]
        public string serviceName { get; set; }
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }

        public Service()
        {
            serviceName = string.Empty;
            startDate = new DateTime();
            endDate = new DateTime();
        }
    }
}
