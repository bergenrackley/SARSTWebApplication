using Microsoft.AspNetCore.Mvc;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;
using System.Data.Entity.Infrastructure;
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
        // Receives data from Register form, checks for duplicates in db, calls AddUser
        [HttpPost]
        public string submitRegistration(string userName, string firstName, string lastName, string email, string password, string userRole)
        {
            // Create new instance of a RegistrationRequest with data from form
            RegistrationRequest newRequest = new RegistrationRequest(userName, firstName, lastName, email, password, userRole);
            
            // Check for duplicate in database tables          
            SarstUser existingUser = _dbContext.SarstUsers.Find(userName);
            RegistrationRequest existingRequest = _dbContext.RegistrationRequests.Find(userName);
            if (existingUser != null) { 
                return "User already exists."; 
            }
            else if(existingRequest != null) { 
                return "Your previous request is still pending."; 
            }
            // If no duplicate, add to RegistrationRequests table
            else return AddUser(newRequest);                
        }    
      
        
        // AddUser
        // Writes the RegistrationRequest to the database table
        public string AddUser(RegistrationRequest newRequest)
        {
            RegistrationRequest newRow = newRequest;
            _dbContext.RegistrationRequests.Add(newRow);
            _dbContext.SaveChanges();
            return HtmlEncoder.Default.Encode($"Added user {newRow.userName} with password {newRow.password}");
        }

    }
}
