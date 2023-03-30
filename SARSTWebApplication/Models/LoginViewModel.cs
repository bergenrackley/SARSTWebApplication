namespace SARSTWebApplication.Models
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            SarstUsers = new List<SarstUser>();
        }

        public List<SarstUser> SarstUsers { get; set; }
    }
}
