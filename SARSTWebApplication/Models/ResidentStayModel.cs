using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class ResidentStayModel
    {

        [Key]
        public ResidentProfile forResident { get; set; }
        public int checkoutDateTime { get; set; }
        public UserProfile provider { get; set; }

        public ResidentStayModel() 
        {
            forResident = new ResidentProfile(); // Resident 
            checkoutDateTime = 0;
            provider = new UserProfile(); // who checks them out
        }
    }
}
