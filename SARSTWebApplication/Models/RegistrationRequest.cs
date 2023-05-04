using SARSTWebApplication.Enums;
using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class RegistrationRequest
    {
        [Key]
        [Display(Name = "User Name")]
        public string userName { get; set; }
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "Password")]
        public string password { get; set; }
        [Display(Name = "Requested User Role")]
        public UserTypes? userRole { get; set; }

        public RegistrationRequest()
        {
            userName = string.Empty;
            firstName = string.Empty;
            lastName = string.Empty;
            email = string.Empty;
            password = string.Empty;
            userRole = null;
        }
    }
}
