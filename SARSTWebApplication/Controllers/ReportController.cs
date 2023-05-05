using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.WebPages;

namespace SARSTWebApplication.Controllers
{
    public class ReportController : BaseController
    {
        private AppDbContext _dbContext;
        private readonly IMemoryCache _cache;
        public ReportController(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
            _cache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }



        void GenerateReportData(ReportModel reportModel)
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

            reportModel.setTitle("Stays over time"); //TODO fix this so it is set based on on table called. 

            reportModel.dataTables["stays"] = GenerateStaysOT(reportModel, stays);
            reportModel.dataTables["services"] = GenerateServicesOT(reportModel, serviceEvents);
            reportModel.dataTables["disciplinaryActions"] = GenerateDisciplineOT(reportModel, disciplinaryEvents);
        }




        //--------------------------------------------------//
        //--------------------Reports-----------------------//
        //--------------------------------------------------//

        //--------------------Stays Over Time-----------------------//
        DataTable GenerateStaysOT(ReportModel reportModel, List<ResidentStay> serviceEvents)
        {

            // Calculate the count of services provided each day between the start and end dates
            Dictionary<DateTime, int> stayCounts = new Dictionary<DateTime, int>();
            DateTime currentDate = reportModel.startDate ?? DateTime.MinValue;
            while (currentDate <= reportModel.endDate)
            {
                int count = serviceEvents.Count(se => se.checkinDateTime.Date == currentDate.Date);
                stayCounts[currentDate] = count;
                currentDate = currentDate.AddDays(1);
            }

            // Create a data table to hold the report data
            DataTable reportData = new DataTable();
            reportData.Columns.Add("Date", typeof(DateTime));
            reportData.Columns.Add("StaysCount", typeof(int));

            // Add the report data to the data table
            foreach (KeyValuePair<DateTime, int> pair in stayCounts)
            {
                DataRow row = reportData.NewRow();
                row["Date"] = pair.Key;
                row["StaysCount"] = pair.Value;
                reportData.Rows.Add(row);
            }

            return reportData;
        }

        //--------------------Services Over Time-----------------------//
        DataTable GenerateServicesOT(ReportModel reportModel, List<ServiceEvent> serviceEvents)
        {

            // Calculate the count of services provided each day between the start and end dates
            Dictionary<DateTime, int> serviceCounts = new Dictionary<DateTime, int>();
            DateTime currentDate = reportModel.startDate ?? DateTime.MinValue;
            while (currentDate <= reportModel.endDate)
            {
                int count = serviceEvents.Count(se => se.dateProvided.Date == currentDate.Date);
                serviceCounts[currentDate] = count;
                currentDate = currentDate.AddDays(1);
            }

            // Create a data table to hold the report data
            DataTable reportData = new DataTable();
            reportData.Columns.Add("Date", typeof(DateTime));
            reportData.Columns.Add("ServiceCount", typeof(int));

            // Add the report data to the data table
            foreach (KeyValuePair<DateTime, int> pair in serviceCounts)
            {
                DataRow row = reportData.NewRow();
                row["Date"] = pair.Key;
                row["ServiceCount"] = pair.Value;
                reportData.Rows.Add(row);
            }

            return reportData;
        }


        //--------------------Disciplinary Actions Over Time-----------------------//
        DataTable GenerateDisciplineOT(ReportModel reportModel, List<DisciplinaryEvent> serviceEvents)
        {

            // Calculate the count of services provided each day between the start and end dates
            Dictionary<DateTime, int> stayCounts = new Dictionary<DateTime, int>();
            DateTime currentDate = reportModel.startDate ?? DateTime.MinValue;
            while (currentDate <= reportModel.endDate)
            {
                int count = serviceEvents.Count(se => se.dateProvided.Date == currentDate.Date);
                stayCounts[currentDate] = count;
                currentDate = currentDate.AddDays(1);
            }

            // Create a data table to hold the report data
            DataTable reportData = new DataTable();
            reportData.Columns.Add("Date", typeof(DateTime));
            reportData.Columns.Add("DisciplineCount", typeof(int));

            // Add the report data to the data table
            foreach (KeyValuePair<DateTime, int> pair in stayCounts)
            {
                DataRow row = reportData.NewRow();
                row["Date"] = pair.Key;
                row["DisciplineCount"] = pair.Value;
                reportData.Rows.Add(row);
            }

            return reportData;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult GenerateReport(ReportModel reportModel, string currentType)
        {
            ReportModel model = new ReportModel();


            if (!_cache.TryGetValue("reportModel", out model))
            {
                GenerateReportData(reportModel);
                // Generate the dataTables dictionary and add it to the cache

                _cache.Set("reportModel", reportModel);
                model = reportModel;
            }

            if (!currentType.IsEmpty())
                model.currentType = currentType;

            byte[] csvData = model.dataTables[model.currentType].ToCSV();
            // Store the data table in a session variable with a key
            HttpContext.Session.Set("downloadData", csvData);

            return View(model);
        }

        // In the Download action
        [HttpGet]
        [HttpPost]
        public FileResult Download()
        {


            // Convert the data table to a CSV byte array
            byte[] csvData = HttpContext.Session.Get("downloadData");

            // Return the file result with the CSV content and the file name
            return File(csvData, "text/csv", "testing.csv");
        }
    }
}
