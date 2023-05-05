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

        public string? residentID { get; set; }
        public Dictionary<string, DataTable> dataTables { get; set; } // Dict of different tables to use in reporting
        public string currentType { get; set; }
        public ReportModel()
        {
            title = "Set Title";
            startDate = null;
            endDate = null;
            dataTables = new Dictionary<string, DataTable>();
            currentType = "stays"; //default report type
        }

        public void setTitle(string title)
        {
            this.title = title;
        }
    }
}
