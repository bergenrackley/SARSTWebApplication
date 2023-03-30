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
            viewModel.SarstUsers = _dbContext.SarstUsers.ToList();
            return View(viewModel);
        }

        // GET: /Login/Register
        // Submit Registration Request
        public IActionResult Register()
        {
            return View("Register");
        }
        
        
        // -------------- Methods --------------------

        // SubmitRegistrationRequest        
        [HttpPost]
        public string submitRegistration(string userName, string firstName, string lastName, string email, string password, string userRole)
        {
            // Check for duplicate in database
            SarstUser existingUser = _dbContext.SarstUsers.Find(userName);
            RegistrationRequest existingRequest = _dbContext.RegistrationRequests.Find(userName);
            if (existingUser != null) { return "user already exists"; }
            else if(existingRequest != null) { return "your previous request is still pending"; }
            else return "call AddUser method to add the request to the queue"; 
               
        }

        //TODO1: Add registration request queue model and update the DB to create table
        //TODO2: Update this method to store all values from the request form in the request queue
      
        public string AddUser(string uName, string fName, string lName, string email, string pword, string uRole)
        {
            var newRow = new RegistrationRequest
            {
                userName = uName,
                firstName = fName,
                lastName = lName,
                email = email,
                password = pword,
                userRole = uRole
            };
            _dbContext.RegistrationRequests.Add(newRow);
            _dbContext.SaveChanges();
            return HtmlEncoder.Default.Encode($"Added user {uName} with password {pword}");
        }

    }
}
