using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SARSTWebApplication.Models
{
    public class ReportModel
    {
        public string title { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Report Start Date")]
        public DateTime? startDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Report End Date")]
        public DateTime? endDate { get; set; }
        public DataTable? residentsTable { get; set; }
        public ReportModel()
        {
            title = "Set Title";
            startDate = null;
            endDate = null;
        }

        public void setTitle(string title)
        {
            this.title = title;
        }
    }
}
