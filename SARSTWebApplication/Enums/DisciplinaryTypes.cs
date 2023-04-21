using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Enums
{
    public enum DisciplinaryTypes
    {
        Warning,
        Education,
        [Display(Name = "Last Chance Contract")]
        LastChanceContract,
        [Display(Name = "Step Away")]
        StepAway
    }
}