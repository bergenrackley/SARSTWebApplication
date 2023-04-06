﻿using Microsoft.AspNetCore.Mvc;
using SARSTWebApplication.Data;
using SARSTWebApplication.Models;
using System.Data.Entity.Infrastructure;
using System.Text.Encodings.Web;

namespace SARSTWebApplication.Controllers
{
    public class LoginController : Controller
    {
        private AppDbContext _dbContext;
        public LoginController(IConfiguration configuration)
        {
            _dbContext = new AppDbContext(configuration.GetConnectionString("DefaultConnection"));
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

        // GET: /Login/Success
        // Registration request submitted confirmation page
        // This is added just for quick access to the view during development
        public IActionResult Success() 
        {
            return View("Success");
        }        
        
        // -------------- Methods --------------------

        // SubmitRegistrationRequest
        // Receives data from Register form, checks for duplicates in db, calls AddRequest
        [HttpPost]
        public IActionResult submitRegistration(string userName, string firstName, string lastName, string email, string password, string userRole)
        {
            // Create new instance of a RegistrationRequest with data from form
            RegistrationRequest newRequest = new RegistrationRequest(userName, firstName, lastName, email, password, userRole);
            
            // Check for duplicate in database tables
            SarstUser existingUser = _dbContext.SarstUsers.Find(userName);
            RegistrationRequest existingRequest = _dbContext.RegistrationRequests.Find(userName);
            
            if (existingUser != null) {
                TempData["Message"] = "Sorry, that username is taken.";
                return Register();
            }
            else if(existingRequest != null) {
                TempData["Message"] = "Be patient. We are still reviewing your previous request.";
                return Register();
            }
            // If no duplicate, add to RegistrationRequests table
            else return AddRequest(newRequest);            
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
            //ViewBag.registrationRequests = _dbContext.RegistrationRequests.ToList();
            return View(_dbContext.RegistrationRequests.ToList());
        }

        public IActionResult editRequest(string id)
        {
            return View(_dbContext.RegistrationRequests.Find(id));
        }

        [HttpPost]
        public RedirectToActionResult editRequest(RegistrationRequest model, string submit) {

            //return model.userName + submit;
            RegistrationRequest request = _dbContext.RegistrationRequests.Find(model.userName);
            if (submit == "Approve")
            {
                _dbContext.SarstUsers.Add(new SarstUser()
                {
                    userName = model.userName,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    password= model.password,
                    userRole= model.userRole,
                    email= model.email
                });
                _dbContext.RegistrationRequests.Remove(request);
                _dbContext.SaveChanges();
            } else if (submit == "Deny")
            {
                _dbContext.RegistrationRequests.Remove(request);
                _dbContext.SaveChanges();
            }
            return RedirectToAction(actionName: "RegistrationRequests");
        }

    }
}
