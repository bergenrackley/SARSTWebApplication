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
        public string userRole { get; set; }

        public RegistrationRequest()
        {
            userName = string.Empty;
            firstName = string.Empty;
            lastName = string.Empty;
            email = string.Empty;
            password = string.Empty;
            userRole = string.Empty;
        }

        public RegistrationRequest(string uName, string fName, string lName, string emailAddress, string pword, string uRole)
        {
            userName =  uName;
            firstName = fName;
            lastName = lName;
            email = emailAddress;
            password = pword;
            userRole = uRole;
        }
    }
}
