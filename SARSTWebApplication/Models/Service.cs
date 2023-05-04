using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class Service
    {
        [Key]
        [Display(Name = "Service Name")]
        public string serviceName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public Nullable<DateTime> startDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public Nullable<DateTime> endDate { get; set; }
        [Display(Name = "Description")]
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
