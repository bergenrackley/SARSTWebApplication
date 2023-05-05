using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
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
        public IActionResult Index(string? residentID)
        {

            if (residentID != null)
                ViewBag.residentID = residentID;


            return View();
        }



        void GenerateReportData(ReportModel reportModel)
        {

            List<ResidentStay> stays = new List<ResidentStay>();
            List<Resident> stayResidents = new List<Resident>();
            List<Resident> residents = new List<Resident>();
            List<ServiceEvent> serviceEvents = new List<ServiceEvent>();
            List<DisciplinaryEvent> disciplinaryEvents = new List<DisciplinaryEvent>();

            //Residents
            residents = _dbContext.Database.SqlQuery<Resident>("Select * from dbo.residents").ToList(); //Returns all residents from stays above


            //Stays
            stays = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckInDateTime >= @start", new SqlParameter("@start", reportModel.startDate)).ToList(); //Returns all stays that started within timeframe regardless of if they ended within timeframe (no real reason this should be a problem)

            //Services and Disciplinary Actions
            if (reportModel.endDate == null || reportModel.endDate >= DateTime.Today) //If no endDate provided or selected date is today or later
            {
                serviceEvents = _dbContext.Database.SqlQuery<ServiceEvent>("Select * from dbo.serviceTracker where dateProvided >= @start", new SqlParameter("@start", reportModel.startDate)).ToList(); //All services provided after startDate
                disciplinaryEvents = _dbContext.Database.SqlQuery<DisciplinaryEvent>("Select * from dbo.disciplinaryTracker where dateProvided >= @start", new SqlParameter("@start", reportModel.startDate)).ToList(); //All disciplinary events after startDate
            }
            else //If endDate provided and its before today
            {
                serviceEvents = _dbContext.Database.SqlQuery<ServiceEvent>("Select * from dbo.serviceTracker where dateProvided >= @start and dateProvided <= @end", new SqlParameter("@start", reportModel.startDate), new SqlParameter("@end", reportModel.endDate)).ToList(); //All services provided between dates
                disciplinaryEvents = _dbContext.Database.SqlQuery<DisciplinaryEvent>("Select * from dbo.disciplinaryTracker where dateProvided >= @start and dateProvided <= @end", new SqlParameter("@start", reportModel.startDate), new SqlParameter("@end", reportModel.endDate)).ToList(); //All disciplinary events between dates
            }

            reportModel.setTitle("Stays over time"); //TODO fix this so it is set based on on table called. 

            reportModel.dataTables["stays"] = GenerateStaysOT(reportModel, stays);
            reportModel.dataTables["services"] = GenerateServicesOT(reportModel, serviceEvents);
            reportModel.dataTables["disciplinaryActions"] = GenerateDisciplineOT(reportModel, disciplinaryEvents);
            reportModel.dataTables["residents"] = GenerateResidents(reportModel, residents);
        }




        //--------------------------------------------------//
        //--------------------Reports-----------------------//
        //--------------------------------------------------//

        //--------------------Stays Over Time-----------------------//
        DataTable GenerateStaysOT(ReportModel reportModel, List<ResidentStay> stays)
        {
            // Filter stays by resident ID if specified
            if (!reportModel.residentID.IsEmpty())
            {
                stays = stays.Where(s => s.residentId == reportModel.residentID).ToList();
            }

            // Calculate the count of stays on each day between the start and end dates,
            // and create a lookup of stay dates to resident IDs
            Dictionary<DateTime, int> stayCounts = new Dictionary<DateTime, int>();
            Dictionary<DateTime, List<string>> stayResidents = new Dictionary<DateTime, List<string>>();
            DateTime currentDate = reportModel.startDate;
            while (currentDate <= reportModel.endDate)
            {
                List<string> residentIds = stays.Where(s => s.checkinDateTime.Date == currentDate.Date)
                                             .Select(s => s.residentId).ToList();
                int count = residentIds.Count;
                stayCounts[currentDate] = count;
                stayResidents[currentDate] = residentIds;
                currentDate = currentDate.AddDays(1);
            }

            // Create a data table to hold the report data
            DataTable reportData = new DataTable();
            reportData.Columns.Add("Date", typeof(DateTime));
            reportData.Columns.Add("StaysCount", typeof(int));
            reportData.Columns.Add("ResidentIds", typeof(string));

            // Add the report data to the data table
            foreach (KeyValuePair<DateTime, int> pair in stayCounts)
            {
                DataRow row = reportData.NewRow();
                row["Date"] = pair.Key;
                row["StaysCount"] = pair.Value;
                row["ResidentIds"] = string.Join(", ", stayResidents[pair.Key]);
                reportData.Rows.Add(row);
            }

            return reportData;
        }



        //--------------------Services Over Time-----------------------//
        DataTable GenerateServicesOT(ReportModel reportModel, List<ServiceEvent> serviceEvents)
        {

            // Filter serviceEvents by resident ID if specified
            if (!reportModel.residentID.IsEmpty())
            {
                serviceEvents = serviceEvents.Where(s => s.residentId == reportModel.residentID).ToList();
            }

            // Calculate the count of services provided each day between the start and end dates
            Dictionary<DateTime, int> serviceCounts = new Dictionary<DateTime, int>();
            DateTime currentDate = reportModel.startDate;
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
        DataTable GenerateDisciplineOT(ReportModel reportModel, List<DisciplinaryEvent> disciplinaryEvents)
        {

            // Filter serviceEvents by resident ID if specified
            if (!reportModel.residentID.IsEmpty())
            {
                disciplinaryEvents = disciplinaryEvents.Where(s => s.residentId == reportModel.residentID).ToList();
            }

            // Calculate the count of services provided each day between the start and end dates
            Dictionary<DateTime, int> stayCounts = new Dictionary<DateTime, int>();
            DateTime currentDate = reportModel.startDate;
            while (currentDate <= reportModel.endDate)
            {
                int count = disciplinaryEvents.Count(se => se.dateProvided.Date == currentDate.Date);
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

        //--------------------Generate Residents-----------------------//
        DataTable GenerateResidents(ReportModel reportModel, List<Resident> residents)
        {
            DataTable residentsTable = new DataTable();
            residentsTable.Columns.Add("Resident Id", typeof(string));
            residentsTable.Columns.Add("First Name", typeof(string));
            residentsTable.Columns.Add("Last Name", typeof(string));
            residentsTable.Columns.Add("Date of Birth", typeof(DateTime));
            residentsTable.Columns.Add("Sex", typeof(string));
            residentsTable.Columns.Add("Gender", typeof(string));
            residentsTable.Columns.Add("Pronouns", typeof(string));
            residentsTable.Columns.Add("Distinguishing Features", typeof(string));
            residentsTable.Columns.Add("Status", typeof(string));

            // Loop through the list of residents and add a new row for each resident
            foreach (var resident in residents)
            {
                DataRow row = residentsTable.NewRow();
                row["Resident Id"] = resident.residentId;
                row["First Name"] = resident.firstName;
                row["Last Name"] = resident.lastName;
                row["Date of Birth"] = resident.dateOfBirth;
                row["Sex"] = resident.sex?.ToString();
                row["Gender"] = resident.gender?.ToString();
                row["Pronouns"] = resident.pronouns?.ToString();
                row["Distinguishing Features"] = resident.distinguishingFeatures;
                row["Status"] = resident.status.ToString();
                residentsTable.Rows.Add(row);
            }

            return residentsTable;
        }


        //--------------------Generate Report Call-----------------------//
        [HttpGet]
        [HttpPost]
        public IActionResult GenerateReport(ReportModel reportModel, string? currentType)
        {
            ReportModel model = new ReportModel();

            if (!currentType.IsNullOrEmpty())
            {

                if (!_cache.TryGetValue("reportModel", out model))
                    model = getCache(reportModel);

                model.currentType = currentType;
            }
            else
            {
                reportModel.currentType = model.currentType;
                model = getCache(reportModel);

            }



            byte[] csvData = model.dataTables[model.currentType].ToCSV();
            // Store the data table in a session variable with a key
            HttpContext.Session.Set("downloadData", csvData);

            return View(model);
        }


        public ReportModel getCache(ReportModel reportModel)
        {
            GenerateReportData(reportModel);
            // Generate the dataTables dictionary and add it to the cache

            _cache.Set("reportModel", reportModel);
            return reportModel;
        }


        //--------------------Download CSV-----------------------//
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
