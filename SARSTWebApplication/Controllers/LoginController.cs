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
        // TODO:
        // The duplicate checking should be moved to a separate method that this method calls.
        // This return type of this method should be changed to an IActionResult that directs the user
        // to different views depending on the the outcome of the method.

        [HttpPost]
        public string submitRegistration(string userName, string firstName, string lastName, string email, string password, string userRole)
        {
            RegistrationRequest newRequest = new RegistrationRequest(userName, firstName, lastName, email, password, userRole);
            // Check for duplicate in database
            SarstUser existingUser = _dbContext.SarstUsers.Find(userName);
            RegistrationRequest existingRequest = _dbContext.RegistrationRequests.Find(userName);
            if (existingUser != null) { return "user already exists"; }
            else if(existingRequest != null) { return "your previous request is still pending"; }
            else return AddUser(newRequest); 
               
        }    
      
        public string AddUser(RegistrationRequest newRequest)
        {
            RegistrationRequest newRow = newRequest;
            _dbContext.RegistrationRequests.Add(newRow);
            _dbContext.SaveChanges();
            return HtmlEncoder.Default.Encode($"Added user {newRow.userName} with password {newRow.password}");
        }

    }
}
