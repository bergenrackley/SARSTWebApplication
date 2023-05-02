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

            reportModel.setTitle("Number of Disciplinary Events");
            //return GenerateTimeline(disciplinaryEvents, reportModel.startDate ?? DateTime.MinValue, reportModel.endDate ?? DateTime.MinValue);
            return GenereateDisciplinaryEventsCount(disciplinaryEvents);

        }


        DataTable GenerateTimeline(List<DisciplinaryEvent> disciplinaryEvents, DateTime startDate, DateTime? endDate)
        {
            // Create a new DataTable to hold the timeline data
            DataTable timelineTable = new DataTable();
            timelineTable.Columns.Add("Date", typeof(DateTime));
            timelineTable.Columns.Add("Count", typeof(int));

            // Loop through each date between the start and end dates
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Count the number of disciplinary events on this date
                int count = disciplinaryEvents.Count(e => e.dateProvided.Date == date);

                // Add a new row to the timeline DataTable
                DataRow row = timelineTable.NewRow();
                row["Date"] = date;
                row["Count"] = count;
                timelineTable.Rows.Add(row);
            }

            return timelineTable;
        }

        //For genereting comparision of all residents to see # of disciplinary actions
        DataTable GenereateDisciplinaryEventsCount(List<DisciplinaryEvent> disciplinaryEvents)
        {
            DataTable disciplinaryTable = new DataTable();
            disciplinaryTable.Columns.Add("ResidentId", typeof(int));
            disciplinaryTable.Columns.Add("Description", typeof(string));
            disciplinaryTable.Columns.Add("dateProvided", typeof(DateTime));

            // Loop through the list of residents and add a new row for each resident
            foreach (var resident in disciplinaryEvents)
            {
                DataRow row = disciplinaryTable.NewRow();
                row["ResidentId"] = resident.residentId;
                row["Description"] = resident.description;
                row["dateProvided"] = resident.dateProvided;
                disciplinaryTable.Rows.Add(row);
            }


            return disciplinaryTable;
        }



        DataTable GenerateResident(List<Resident> residents)
        {

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

        [HttpGet]
        [HttpPost]
        public IActionResult GenerateReport(ReportModel reportModel)
        {
            DataTable residentsTable = GenerateReportData(reportModel);
            byte[] csvData = residentsTable.ToCSV();
            // Store the data table in a session variable with a key
            HttpContext.Session.Set("residentsTable", csvData);


            reportModel.residentsTable = residentsTable;
            ViewBag.csvData = csvData;
            return View(reportModel);
        }

        // In the Download action
        [HttpGet]
        [HttpPost]
        public FileResult Download()
        {


            // Convert the data table to a CSV byte array
            byte[] csvData = HttpContext.Session.Get("residentsTable");

            // Return the file result with the CSV content and the file name
            return File(csvData, "text/csv", "testing.csv");
        }
    }
}
