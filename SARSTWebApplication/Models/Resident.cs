using SARSTWebApplication.Enums;
using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class Resident
    {
        [Key]
        public string residentId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? dateOfBirth { get; set; }
        public ResidentSex? sex { get; set; }
        public ResidentGender? gender { get; set; }
        public ResidentPronouns? pronouns { get; set; }
        public string distinguishingFeatures { get; set; }
        public DisciplinaryTypes status { get; set; }

        public Resident()
        {
            residentId = string.Empty;
            firstName = string.Empty;
            lastName = string.Empty;
            dateOfBirth = null;
            sex = null;
            gender = null;
            pronouns = null;
            distinguishingFeatures = string.Empty;
            status = DisciplinaryTypes.GoodStanding;
        }
    }
}