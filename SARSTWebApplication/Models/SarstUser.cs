using SARSTWebApplication.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class SarstUser
    {
        [Key]
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public UserTypes? userRole { get; set; }
        [DefaultValue(0)]
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
