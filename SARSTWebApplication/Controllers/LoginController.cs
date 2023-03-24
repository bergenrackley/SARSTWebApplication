using Microsoft.AspNetCore.Mvc;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;
using System.Text.Encodings.Web;

namespace SARSTWebApplication.Controllers
{
    public class LoginController : Controller
    {
        private AppDbContext _dbContext;
        public LoginController()
        {
            _dbContext = new AppDbContext("Server=tcp:sarst.database.windows.net,1433;Initial Catalog=sarst;Persist Security Info=False;User ID=sarstadmin;Password=LarryBoy11;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
        public IActionResult Index()
        {
            var viewModel = new LoginViewModel();
            viewModel.Users = _dbContext.Users.ToList();
            return View(viewModel);
        }

        public string AddUser(string uName, string pword)
        {
            var newRow = new UserProfile
            {
                userName = uName,
                password = pword
            };
            _dbContext.Users.Add(newRow);
            _dbContext.SaveChanges();
            return HtmlEncoder.Default.Encode($"Added user {uName} with password {pword}");
        }

    }
}
