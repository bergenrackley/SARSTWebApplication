﻿using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class ServiceModel
    {
        [Key]
        public int serviceID { get; set; }
        [DataType(DataType.Date)]
        public DateTime dateTime { get; set; }
        public SarstUser serviceProvider { get; set; }
        public ResidentStayModel duringVisit { get; set; }

        public ServiceModel()
        {
            serviceID = new int(); // FIXME
            dateTime = new DateTime();
            serviceProvider = new SarstUser();
            duringVisit = new ResidentStayModel();
        }
    }

   
}
