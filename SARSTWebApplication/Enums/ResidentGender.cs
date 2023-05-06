using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Enums
{
    public enum ResidentGender
    {
        [Display(Name = "N/A")]
        NA,
        M,
        F,
        Transgender,
        [Display(Name = "Non-binary")]
        Nonbinary,
        [Display(Name = "Gender Fluid")]
        GenderFluid,
        Other
    }
}