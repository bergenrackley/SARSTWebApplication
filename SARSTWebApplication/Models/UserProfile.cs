using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class UserProfile
    {
        [Key]
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string userRole { get; set; }

        public UserProfile()
        {
            userName = string.Empty;
            firstName = string.Empty;
            lastName = string.Empty;
            email = string.Empty;
            password = string.Empty;
            userRole = string.Empty;
        }
    }
}
