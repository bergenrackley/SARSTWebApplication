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
            _dbContext = new AppDbContext("Server=tcp:sarst.database.windows.net,1433;Initial Catalog=sarst;Persist Security Info=False;User ID=sarstadmin;Password=S4rstP4ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
        public IActionResult Index()
        {
            var viewModel = new LoginViewModel();
            viewModel.Users = _dbContext.Users.ToList();
            return View(viewModel);
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public string submitRegistration(string userName, string firstName, string lastName, string email, string password, string userRole)
        {
            SarstUser existingUser = _dbContext.Users.Find(userName);
            if (existingUser != null) { return "user already exists"; }
            else return "new user";
        }

        public string AddUser(string uName, string pword)
        {
            var newRow = new SarstUser
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
