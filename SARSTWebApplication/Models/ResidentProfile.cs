﻿using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class ResidentProfile
    {
        [Key]
        public string residentId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime dateOfBirth { get; set; }
        public string sex { get; set; }
        public string gender { get; set; }
        public string pronouns { get; set; }
        public string distinguishingFeatures { get; set; }
        public string status { get; set; }

        public ResidentProfile()
        {
            residentId = string.Empty;
            firstName = string.Empty;
            lastName = string.Empty;
            dateOfBirth= new DateTime();
            sex = string.Empty;
            gender = string.Empty;
            pronouns = string.Empty;
            distinguishingFeatures= string.Empty;
            status = string.Empty;
        }
    }
}
