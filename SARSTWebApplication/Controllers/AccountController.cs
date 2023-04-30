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
        public AccountController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
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
            }
            else if (submit == "Deny")
            {
                _dbContext.RegistrationRequests.Remove(request);
                _dbContext.SaveChanges();
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

    }
}
