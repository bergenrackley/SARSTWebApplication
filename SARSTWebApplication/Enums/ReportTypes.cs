using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Enums
{
    public enum ReportTypes
    {
        Stays,
        Services,
        [Display(Name = "Disciplinary Actions")]
        DActions,
        Residents,
        All
    }
}