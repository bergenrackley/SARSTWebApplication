using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class ResidentStayModel
    {

        [Key]
        public int stayId { get; set; }
        public ResidentProfile forResident { get; set; }
        [DataType(DataType.Date)]
        public DateTime checkinDateTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime checkoutDateTime { get; set; }
        public SarstUser provider { get; set; }

        public ResidentStayModel() 
        {
            stayId = new int();
            forResident = new ResidentProfile(); // Resident 
            checkinDateTime = new DateTime();
            checkoutDateTime = new DateTime();
            provider = new SarstUser(); // who checks them out
        }
    }
}
