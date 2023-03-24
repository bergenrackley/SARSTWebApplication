namespace SARSTWebApplication.Models
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            Users = new List<UserProfile>();
        }

        public List<UserProfile> Users { get; set; }
    }
}
