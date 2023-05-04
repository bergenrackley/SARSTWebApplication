using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Enums
{
    public enum DisciplinaryTypes
    {
        [Display(Name = "Good Standing")]
        GoodStanding,
        Warning,
        Education,
        [Display(Name = "Last Chance Contract")]
        LastChanceContract,
        [Display(Name = "Step Away")]
        StepAway
    }
}