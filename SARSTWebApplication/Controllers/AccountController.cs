using Azure;
using Azure.Communication.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SARSTWebApplication.Data;
using SARSTWebApplication.Enums;
using SARSTWebApplication.Models;

namespace SARSTWebApplication.Controllers
{
    public class AccountController : BaseController
    {
        private AppDbContext _dbContext;
        private EmailClient emailClient;
        public AccountController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
            string connectionString = configuration.GetConnectionString("EmailConnection");
            emailClient = new EmailClient(connectionString);
        }

        // GET: /Account/SARST
        // Landing Page After Successfully Logging In
        public IActionResult SARST()
        {
            ViewBag.currentUserRole = HttpContext.Session.GetInt32("userRole");
            ViewBag.currentUserName = HttpContext.Session.GetString("userName");
            ViewBag.currentUserRoleName = (UserTypes)HttpContext.Session.GetInt32("userRole");
            return View();
        }

        public IActionResult registrationRequests()
        {
            return View(_dbContext.RegistrationRequests.ToList());
        }

        public IActionResult editRequest(string id)
        {
            return View(_dbContext.RegistrationRequests.Find(id));
        }

        [HttpPost]
        public RedirectToActionResult editRequest(RegistrationRequest model, string submit)
        {
            RegistrationRequest request = _dbContext.RegistrationRequests.Find(model.userName);

            if (submit == "Approve")
            {
                _dbContext.SarstUsers.Add(new SarstUser()
                {
                    userName = request.userName,
                    firstName = request.firstName,
                    lastName = request.lastName,
                    password = request.password,
                    userRole = request.userRole,
                    email = request.email
                });
                _dbContext.RegistrationRequests.Remove(request);
                _dbContext.SaveChanges();
                sendEmail(model.email, "Your Registration Request for a SARST account has been approved", $"<html><body><h1>SARST Registration Successful</h1><br/><h4>Your Registration Request for a SARST account has been approved. As a reminder, your username is {model.userName} </h4></body></html>");
            }
            else if (submit == "Deny")
            {
                _dbContext.RegistrationRequests.Remove(request);
                _dbContext.SaveChanges();
                sendEmail(model.email, "Your Registration Request for a SARST account has been denied", "<html><body><h1>SARST Registration Denied</h1><br/><h4>Your Registration Request for a SARST account has been denied. Please contact your admin for more information.</h4></body></html>");
            }
            return RedirectToAction(actionName: "RegistrationRequests");
        }

        public IActionResult ChangePassword()
        {
            return View(_dbContext.SarstUsers.Find(HttpContext.Session.GetString("userName")));
        }

        [HttpPost]
        public string ChangePassword(string newPassword, string confirmPassword)
        {
            if (newPassword == confirmPassword && HttpContext.Session.GetString("userName") != null)
            {
                SarstUser currentUser = _dbContext.SarstUsers.Find(HttpContext.Session.GetString("userName"));
                newPassword = PasswordManager.HashPassword(newPassword);
                currentUser.password = newPassword;
                currentUser.changePassword = 0;
                _dbContext.SaveChanges();
                sendEmail(currentUser.email, "Your SARST account password has been changed", $"<html><body><h1>SARST Password Reset</h1><br/><h4>Your SARST Account password has been changed. If you did not make this change, please contact your Root User.</h4></body></html>");
                return "Success";
            }
            else return "Passwords do not match";
        }

        public IActionResult SarstUsers()
        {
            return View(_dbContext.SarstUsers.ToList());
        }

        public IActionResult EditSarstUser(string userName)
        {
            return View(_dbContext.SarstUsers.Find(userName));
        }

        [HttpPost]
        public string ResetUserPassword(string userName)
        {
            string newPassword = Guid.NewGuid().ToString().Replace("-", "");
            string hashPassword = PasswordManager.HashPassword(newPassword);
            SarstUser user = _dbContext.SarstUsers.Find(userName);
            user.password = hashPassword;
            user.changePassword = 1;
            _dbContext.SaveChanges();
            sendEmail(user.email, "Your SARST account password has been reset", $"<html><body><h1>SARST Password Reset</h1><br/><h4>Your SARST Account password has been reset. It is now '{newPassword}'.</h4></body></html>");
            return $"The password for user '{user.userName}' has been changed. It is now '{newPassword}'.";
        }

        [HttpDelete]
        public string DeleteUser(string userName)
        {
            _dbContext.SarstUsers.Remove(_dbContext.SarstUsers.Find(userName));
            _dbContext.SaveChanges();
            return "Success";
        }

        // User Log Out
        // Clears session variables and returns to Account/Index
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(actionName: "Index", controllerName: "public");
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
    }
}
