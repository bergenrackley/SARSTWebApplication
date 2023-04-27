using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;
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

        [HttpGet]
        public string GenerateReport(ReportModel reportModel)
        {
            List<ResidentStay> stays = new List<ResidentStay>();
            List<ServiceEvent> serviceEvents = new List<ServiceEvent>();
            List<DisciplinaryEvent> disciplinaryEvents = new List<DisciplinaryEvent>();
            if (reportModel.endDate == null)
            {
                stays = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckInDateTime > @start", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980"))).ToList();
                serviceEvents = _dbContext.Database.SqlQuery<ServiceEvent>("Select * from dbo.serviceTracker as t inner join dbo.residentStays as s on t.stayId = s.stayId where s.CheckInDateTime >= @start", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980"))).ToList();
                disciplinaryEvents = _dbContext.Database.SqlQuery<DisciplinaryEvent>("Select * from dbo.disciplinaryTracker as t inner join dbo.residentStays as s on t.stayId = s.stayId where s.CheckInDateTime >= @start", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980"))).ToList();
            }
            else
            {
                stays = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckInDateTime > @start and CheckOutDateTime < @end", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980")), new SqlParameter("@end", reportModel.endDate)).ToList();
                serviceEvents = _dbContext.Database.SqlQuery<ServiceEvent>("Select * from dbo.serviceTracker as t inner join dbo.residentStays as s on t.stayId = s.stayId where s.CheckInDateTime >= @start and s.CheckOutDateTime <= @end", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980")), new SqlParameter("@end", reportModel.endDate)).ToList();
                disciplinaryEvents = _dbContext.Database.SqlQuery<DisciplinaryEvent>("Select * from dbo.disciplinaryTracker as t inner join dbo.residentStays as s on t.stayId = s.stayId where s.CheckInDateTime >= @start and s.CheckOutDateTime <= @end", new SqlParameter("@start", reportModel.startDate ?? DateTime.Parse("1/1/1980")), new SqlParameter("@end", reportModel.endDate)).ToList();
            }
            return disciplinaryEvents.ToJson();
        }
    }
}
