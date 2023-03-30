namespace SARSTWebApplication.Models
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            Users = new List<SarstUser>();
        }

        public List<SarstUser> Users { get; set; }
    }
}
