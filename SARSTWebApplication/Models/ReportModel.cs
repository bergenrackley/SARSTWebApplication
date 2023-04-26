using System.ComponentModel.DataAnnotations;
namespace SARSTWebApplication.Models
{
    public class ReportModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Report Start Date")]
        public DateTime? startDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Report End Date")]
        public DateTime? endDate { get; set; }

        public ReportModel()
        {
            startDate = null;
            endDate = null;
        }
    }
}
