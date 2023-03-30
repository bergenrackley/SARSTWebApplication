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
        
        // GET: /Login
        // Landing Page 
        // Right now it is displaying a list of users from the database
        public IActionResult Index()
        {
            var viewModel = new LoginViewModel();
            viewModel.Users = _dbContext.Users.ToList();
            return View(viewModel);
        }

        // GET: /Login/Register
        // Submit Registration Request
        public IActionResult Register()
        {
            return View("Register");
        }
        
        // SubmitRegistrationRequest        
        [HttpPost]
        public string submitRegistration(string userName, string firstName, string lastName, string email, string password, string userRole)
        {
            // Check for duplicate
            SarstUser existingUser = _dbContext.Users.Find(userName);
            if (existingUser != null) { return "user already exists"; }
            else return "call AddUser method to add the request to the queue"; 

            // If no duplicate, add user request information to registration request queue
        }

        //TODO1: Add registration request queue model and update the DB to create table
        //TODO2: Update this method to store all values from the request form in the request queue
      
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
