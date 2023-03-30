using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class ServiceModel
    {
        [Key]
        public int serviceID { get; set; }
        public int dateTime { get; set; }
        public UserProfile serviceProvider { get; set; }
        public ResidentStayModel duringVisit { get; set; }

        public ServiceModel()
        {
            serviceID = 0; // FIXME
            dateTime = 0;
            serviceProvider = new UserProfile();
            duringVisit = new ResidentStayModel();
        }
    }

   
}
