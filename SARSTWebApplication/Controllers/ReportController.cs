using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            
            List<ResidentStay> result = _dbContext.Database.SqlQuery<ResidentStay>("Select * from dbo.residentStays where CheckInDateTime > @start", new SqlParameter("@start", reportModel.startDate ?? DateTime.MinValue)).ToList();
            return result.ToJson();
        }
    }
}
