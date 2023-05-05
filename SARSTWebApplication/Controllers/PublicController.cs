using Azure;
using Azure.Communication.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.userTypes = getUserTypes();
            return View("Register");
        }

        [HttpPost]
        public IActionResult Register(RegistrationRequest model) //When Ajax calls are figured out, will need to change to RedirectToActionResult return instead of IActionResult
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
                return Register();
            }

            if (existingUser != null)
            {
                TempData["Message"] = "Sorry, that username is taken.";
                return Register();
            }

            if (existingRequest != null)
            {
                TempData["Message"] = "Be patient. We are still reviewing your previous request.";
                return Register();
            }
            // If no duplicate, add to RegistrationRequests table
            _dbContext.RegistrationRequests.Add(model);
            _dbContext.SaveChanges();
            sendEmail(model.email);
            return RedirectToAction(actionName: "Success");
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
        public IActionResult LoginAttempt(SarstUser sarstUser)
        {
            SarstUser validUser = _dbContext.SarstUsers.Find(sarstUser.userName);
            if (validUser != null)
            {
                if (validUser.password == sarstUser.password)
                {
                    HttpContext.Session.SetString("userName", validUser.userName);
                    HttpContext.Session.SetInt32("userRole", (int)validUser.userRole);

                    if (validUser.changePassword == 1) return RedirectToAction(actionName: "ChangePassword");
                    else return RedirectToAction(actionName: "SARST", controllerName: "Account");
                }
            }
            return RedirectToAction(actionName: "Login");
        }

        public IActionResult SARST()
        {
            //These add the session values to the view
            // Is a user logged in?
            // currentUserRole will be 0,1, or 2 if a user is logged in
            if (ViewBag.currentUserRole > -1)
            {
                return RedirectToAction(actionName: "SARST", controllerName: "Account");
            }
            else
            {
                return RedirectToAction(actionName: "Index");
            }

        }

        [HttpPost]
        public async void sendEmail(string recipient)
        {
            var subject = "Your Registration Request for a SARST account has been recieved";
            var htmlContent = "<html><body><h1>SARST Registration Request Successful</h1><br/><h4>Your Registration Request for a SARST account has been recieved and is being processed.</h4></body></html>";
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
        public List<SelectListItem> getUserTypes()
        {
            return Enum.GetValues(typeof(UserTypes)).Cast<UserTypes>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
        }
    }
}
