using SARSTWebApplication.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class SarstUser
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
        [DefaultValue(0)]
        [Display(Name = "Need to change password?")]
        public int changePassword { get; set; }

        public SarstUser()
        {
            userName = string.Empty;
            firstName = string.Empty;
            lastName = string.Empty;
            email = string.Empty;
            password = string.Empty;
            userRole = null;
            changePassword = 0;
        }
    }
}
