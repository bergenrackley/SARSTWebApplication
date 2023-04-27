using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SARSTWebApplication.Data;
using SARSTWebApplication.Enums;
using SARSTWebApplication.Models;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace SARSTWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext _dbContext;
        public AccountController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
        }

        // GET: /Login
        // Landing Page 
        // Right now it is displaying a list of users from the database
        public IActionResult Index()
        {
            ViewBag.SarstUsers = _dbContext.SarstUsers.ToList();
            ViewBag.currentUserName = HttpContext.Session.GetString("userName");
            // Adding userRole session value for managing access to funtionality -CJ
            // userRole is an int. Levels: 0=Root, 1=Admin, 2=Assistant
            ViewBag.currentUserRole = HttpContext.Session.GetInt32("userRole");

            return View();
        }

        // GET: /Login/Register
        // Submit Registration Request


        public IActionResult Register()
        {
            ViewBag.userTypes = getUserTypes();
            return View("Register");
        }

        // GET: /Login/Success
        // Registration request submitted confirmation page
        // This is added just for quick access to the view during development
        public IActionResult Success()
        {
            return View("Success");
        }

        // GET: /Account/SARST
        // Landing Page After Successfully Logging In
        public IActionResult SARST()
        {
            //These add the session values to the view
            ViewBag.currentUserRole = HttpContext.Session.GetInt32("userRole");
            ViewBag.currentUserName = HttpContext.Session.GetString("userName");
            // Is a user logged in?
            // currentUserRole will be 0,1, or 2 if a user is logged in
            if (ViewBag.currentUserRole > -1)
            {
                ViewBag.currentUserRoleName = (UserTypes)HttpContext.Session.GetInt32("userRole");
                return View("SARST");
            }
            else
            {
                return View("Index");
            }

        }

        // GET: /Account/Login
        // Login page
        public IActionResult Login()
        {
            return View();
        }

        // -------------- Methods --------------------

        // SubmitRegistrationRequest
        // Receives data from Register form, checks for duplicates in db, calls AddRequest
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
            return AddRequest(model);
        }


        // AddUser
        // Writes the RegistrationRequest to the database table and loads the Success view
        public IActionResult AddRequest(RegistrationRequest newRequest)
        {
            RegistrationRequest newRow = newRequest;
            _dbContext.RegistrationRequests.Add(newRow);
            _dbContext.SaveChanges();
            return Success();
            /*HtmlEncoder.Default.Encode($"Added user {newRow.userName} with password {newRow.password}");*/
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



        [HttpPost]
        public RedirectToActionResult LoginAttempt(SarstUser sarstUser)
        {
            SarstUser validUser = _dbContext.SarstUsers.Find(sarstUser.userName);
            if (validUser != null)
            {
                if (validUser.password == sarstUser.password)
                {
                    HttpContext.Session.SetString("userName", validUser.userName);
                    HttpContext.Session.SetInt32("userRole", (int)validUser.userRole);

                    if (validUser.changePassword == 1) return RedirectToAction(actionName: "ChangePassword");
                    else return RedirectToAction(actionName: "SARST");
                }
            }
            return RedirectToAction(actionName: "Login");
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

        // User Log Out
        // Clears session variables and returns to Account/Index
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return View("Index");
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
