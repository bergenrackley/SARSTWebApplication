using SARSTWebApplication.Enums;
using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class RegistrationRequest
    {
        [Key]
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
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
