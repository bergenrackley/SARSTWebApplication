using SARSTWebApplication.Enums;
using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Models
{
    public class Resident
    {
        [Key]
        [Display(Name = "Resident Id")]
        public string residentId { get; set; }
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? dateOfBirth { get; set; }
        [Display(Name = "Sex")]
        public ResidentSex? sex { get; set; }
        [Display(Name = "Gender")]
        public ResidentGender? gender { get; set; }
        [Display(Name = "Pronouns")]
        public ResidentPronouns? pronouns { get; set; }
        [Display(Name = "Distinguishing Features")]
        public string distinguishingFeatures { get; set; }
        [Display(Name = "Status")]
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