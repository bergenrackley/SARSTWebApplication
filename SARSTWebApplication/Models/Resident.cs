using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SARSTWebApplication.Models
{
    public class Resident
    {
        [Key]
        public string residentId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string sex { get; set; }
        public string gender { get; set; }
        public string pronouns { get; set; }
        public string distinguishingFeatures { get; set; }
        public string status { get; set; }

        public Resident()
        {
            residentId = string.Empty;
            firstName = string.Empty;
            lastName = string.Empty;
            dateOfBirth = DateTime.Now;
            sex = string.Empty;
            gender = string.Empty;
            pronouns = string.Empty;
            distinguishingFeatures = string.Empty;
            status = string.Empty;
        }
    }
}