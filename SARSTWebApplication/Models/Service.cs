using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class Service
    {
        [Key]
        public string serviceName { get; set; }
        public Nullable<DateTime> startDate { get; set; }
        public Nullable<DateTime> endDate { get; set; }

        public Service()
        {
            serviceName = string.Empty;
            startDate = null;
            endDate = null;
        }
    }
}
