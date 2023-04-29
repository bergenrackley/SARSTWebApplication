using Microsoft.AspNetCore.Mvc;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;
using System.Data;
using System.Data.SqlClient;

namespace SARSTWebApplication.Controllers
{
    public class ReportController : BaseController
    {
        private AppDbContext _dbContext;

        public ReportController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
        }
        public IActionResult Index()
        {
            return View();
        }



        DataTable GenerateReportData(ReportModel reportModel)
        {

            List<ResidentStay> stays = new List<ResidentStay>();
            List<Resident> residents = new List<Resident>();
            List<ServiceEvent> serviceEvents = new List<ServiceEvent>();
            List<DisciplinaryEvent> disciplinaryEvents = new List<DisciplinaryEvent>();
            stays = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckInDateTime >= @start", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980"))).ToList(); //Returns all stays that started within timeframe regardless of if they ended within timeframe (no real reason this should be a problem)
            residents = _dbContext.Database.SqlQuery<Resident>("Select * from dbo.residents where residentId in (Select residentId from dbo.residentStays where CheckInDateTime >= @start)", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980"))).ToList(); //Returns all residents from stays above
            if (reportModel.endDate == null || reportModel.endDate >= DateTime.Today) //If no endDate provided or selected date is today or later
            {
                serviceEvents = _dbContext.Database.SqlQuery<ServiceEvent>("Select * from dbo.serviceTracker where dateProvided >= @start", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980"))).ToList(); //All services provided after startDate
                disciplinaryEvents = _dbContext.Database.SqlQuery<DisciplinaryEvent>("Select * from dbo.disciplinaryTracker where dateProvided >= @start", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980"))).ToList(); //All disciplinary events after startDate
            }
            else //If endDate provided and its before today
            {
                serviceEvents = _dbContext.Database.SqlQuery<ServiceEvent>("Select * from dbo.serviceTracker where dateProvided >= @start and dateProvided <= @end", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980")), new SqlParameter("@end", reportModel.endDate)).ToList(); //All services provided between dates
                disciplinaryEvents = _dbContext.Database.SqlQuery<DisciplinaryEvent>("Select * from dbo.disciplinaryTracker where dateProvided >= @start and dateProvided <= @end", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980")), new SqlParameter("@end", reportModel.endDate)).ToList(); //All disciplinary events between dates
            }

            DataTable residentsTable = new DataTable();
            residentsTable.Columns.Add("ResidentId", typeof(int));
            residentsTable.Columns.Add("FirstName", typeof(string));
            residentsTable.Columns.Add("LastName", typeof(string));
            residentsTable.Columns.Add("BirthDate", typeof(DateTime));

            // Loop through the list of residents and add a new row for each resident
            foreach (var resident in residents)
            {
                DataRow row = residentsTable.NewRow();
                row["ResidentId"] = resident.residentId;
                row["FirstName"] = resident.firstName;
                row["LastName"] = resident.lastName;
                row["BirthDate"] = resident.dateOfBirth;
                residentsTable.Rows.Add(row);
            }

            return residentsTable;
        }

        [HttpPost]
        public FileResult GenerateReport(ReportModel reportModel)
        {
            DataTable residentsTable = GenerateReportData(reportModel);

            byte[] csvData = residentsTable.ToCSV();
            return File(csvData, "text/csv", "residents.csv");
        }
    }
}
