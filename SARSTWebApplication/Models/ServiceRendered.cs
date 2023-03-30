﻿using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class ServiceRendered
    {
        [Key]
        public int serviceID { get; set; }
        [DataType(DataType.Date)]
        public DateTime dateTime { get; set; }
        public string providerUserName { get; set; }
        public int stayId { get; set; }

        public ServiceRendered()
        {
            serviceID = new int(); // FIXME
            dateTime = new DateTime();
            providerUserName = String.Empty;
            stayId = new int();
        }
    }

   
}
