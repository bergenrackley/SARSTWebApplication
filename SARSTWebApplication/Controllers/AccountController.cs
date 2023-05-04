using Azure.Communication.Email;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SARSTWebApplication.Data;
using SARSTWebApplication.Enums;
using SARSTWebApplication.Models;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

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
                    userName = model.userName,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    password = model.password,
                    userRole = model.userRole,
                    email = model.email
                });
                _dbContext.RegistrationRequests.Remove(request);
                _dbContext.SaveChanges();
                sendEmail(model.email, true, model.userName);
            }
            else if (submit == "Deny")
            {
                _dbContext.RegistrationRequests.Remove(request);
                _dbContext.SaveChanges();
                sendEmail(model.email, false);
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
                currentUser.password = newPassword;
                currentUser.changePassword = 0;
                _dbContext.SaveChanges();
                return "Success";
            }
            else return "Error";
        }

        public IActionResult SarstUsers()
        {
            return View(_dbContext.SarstUsers.ToList());
        }

        public IActionResult EditSarstUser(string userName) {
            return View(_dbContext.SarstUsers.Find(userName));
        }

        [HttpPost]
        public string ResetUserPassword(string userName)
        {
            string newPassword = Guid.NewGuid().ToString().Replace("-", "");
            SarstUser user = _dbContext.SarstUsers.Find(userName);
            user.password = newPassword;
            user.changePassword = 1;
            _dbContext.SaveChanges();
            return newPassword;
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
        public async void sendEmail(string recipient, bool approved, string? userName = null)
        {
            string subject;
            string htmlContent;
            if (approved)
            {
                subject = "Your Registration Request for a SARST account has been approved";
                htmlContent = $"<html><body><h1>SARST Registration Successful</h1><br/><h4>Your Registration Request for a SARST account has been approved. As a reminder, your username is {userName} </h4></body></html>";
            } else
            {
                subject = "Your Registration Request for a SARST account has been denied";
                htmlContent = "<html><body><h1>SARST Registration Denied</h1><br/><h4>Your Registration Request for a SARST account has been denied. Please contact your admin for more information.</h4></body></html>";
            }
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
