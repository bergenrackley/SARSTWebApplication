using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class ResidentStayModel
    {

        [Key]
        public int visitId { get; set; }
        public ResidentProfile forResident { get; set; }
        [DataType(DataType.Date)]
        public DateTime checkinDateTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime checkoutDateTime { get; set; }
        public UserProfile provider { get; set; }

        public ResidentStayModel() 
        {
            visitId = new int();
            forResident = new ResidentProfile(); // Resident 
            checkinDateTime = new DateTime();
            checkoutDateTime = new DateTime();
            provider = new UserProfile(); // who checks them out
        }
    }
}
