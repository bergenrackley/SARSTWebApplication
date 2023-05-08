using Azure;
using Azure.Communication.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using NuGet.Protocol;
using SARSTWebApplication.Data;
using SARSTWebApplication.Enums;
using SARSTWebApplication.Models;

namespace SARSTWebApplication.Controllers
{
    public class PublicController : Controller
    {
        private AppDbContext _dbContext;
        private EmailClient emailClient;
        public PublicController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
            string connectionString = configuration.GetConnectionString("EmailConnection");
            emailClient = new EmailClient(connectionString);
        }

        public IActionResult Index()
        {
            ViewBag.SarstUsers = _dbContext.SarstUsers.ToList();
            ViewBag.currentUserName = HttpContext.Session.GetString("userName");
            // Adding userRole session value for managing access to funtionality -CJ
            // userRole is an int. Levels: 0=Root, 1=Admin, 2=Assistant
            ViewBag.currentUserRole = HttpContext.Session.GetInt32("userRole");

            return View();
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public string Register(RegistrationRequest model) //When Ajax calls are figured out, will need to change to RedirectToActionResult return instead of IActionResult
        {
            // Create new instance of a RegistrationRequest with data from form
            //RegistrationRequest newRequest = new RegistrationRequest(userName, firstName, lastName, email, password, userRole);

            // Check for duplicate in database tables
            SarstUser existingUser = _dbContext.SarstUsers.Find(model.userName);
            RegistrationRequest existingRequest = _dbContext.RegistrationRequests.Find(model.userName);


            // Check if the model is valid (like empty fields)
            if (!ModelState.IsValid)
            {
                // Return the registration page with validation errors
                return getErrors(ModelState);
            }

            if (existingUser != null)
            {
                return "Sorry, that username is taken.";
            }

            if (existingRequest != null)
            {
                return "Be patient. We are still reviewing your previous request.";
            }
            // If no duplicate, add to RegistrationRequests table
            model.password = PasswordManager.HashPassword(model.password);
            _dbContext.RegistrationRequests.Add(model);
            _dbContext.SaveChanges();
            sendEmail(model.email, "Your Registration Request for a SARST account has been recieved", "<html><body><h1>SARST Registration Request Successful</h1><br/><h4>Your Registration Request for a SARST account has been recieved and is being processed.</h4></body></html>");
            return "Success";
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public string LoginAttempt(SarstUser sarstUser)
        {
            SarstUser validUser = _dbContext.SarstUsers.Find(sarstUser.userName);
            if (validUser != null)
            {
                string attemptPass = PasswordManager.HashPassword(sarstUser.password);
                if (validUser.password == attemptPass)
                {
                    HttpContext.Session.SetString("userName", validUser.userName);
                    HttpContext.Session.SetInt32("userRole", (int)validUser.userRole);

                    if (validUser.changePassword == 1) return "ChangePassword";
                    else return "SARST";
                }
            }
            return "Username and password do not match";
        }

        public IActionResult ForgotPassword() { 
            return View(); 
        }

        [HttpPost]
        public string ForgotPassword(SarstUser forgotUser)
        {
            SarstUser sarstUser = _dbContext.SarstUsers.Find(forgotUser.userName);
            if (sarstUser != null && sarstUser.email == forgotUser.email)
            {
                string newPassword = Guid.NewGuid().ToString().Replace("-", "");
                newPassword = PasswordManager.HashPassword(newPassword);
                sarstUser.password = newPassword;
                sarstUser.changePassword = 1;
                _dbContext.SaveChanges();
                sendEmail(sarstUser.email, "Your SARST account password has been reset", $"<html><body><h1>SARST Password Reset</h1><br/><h4>Your SARST Account password has been reset. It is now '{newPassword}'.</h4></body></html>");
                return $"The password for user '{sarstUser.userName}' has been reset. Please check your email.";
            }
            else return "Username and Password do not match.";
        }

        [HttpPost]
        public async void sendEmail(string recipient, string subject, string htmlContent)
        {
            var sender = "DoNotReply@183f41b5-f499-409b-a97a-40bff5159504.azurecomm.net";

            try
            {
                Console.WriteLine("Sending email...");
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                    Azure.WaitUntil.Completed,
                    sender,
                    recipient,
                    subject,
                    htmlContent);
                EmailSendResult statusMonitor = emailSendOperation.Value;

                Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                string operationId = emailSendOperation.Id;
                Console.WriteLine($"Email operation id = {operationId}");
            }
            catch (RequestFailedException ex)
            {
                /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
            }
        }

        [NonAction]
        public string getErrors(ModelStateDictionary modelState)
        {
            string errors = string.Empty;
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (ModelError error in allErrors)
            {
                errors += $"{error.ErrorMessage}\n";
            }
            return errors;
        }
    }
}
