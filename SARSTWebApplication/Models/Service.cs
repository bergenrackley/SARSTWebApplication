using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class Service
    {
        [Key]
        public string serviceName { get; set; }
        [DataType(DataType.Date)]
        public Nullable<DateTime> startDate { get; set; }
        [DataType(DataType.Date)]
        public Nullable<DateTime> endDate { get; set; }
        public string description { get; set; }

        public Service()
        {
            serviceName = string.Empty;
            startDate = null;
            endDate = null;
            description = string.Empty;
        }
    }
}
