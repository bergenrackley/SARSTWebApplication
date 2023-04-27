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

        public Service()
        {
            serviceName = string.Empty;
            startDate = null;
            endDate = null;
        }
    }
}
