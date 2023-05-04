using System.ComponentModel.DataAnnotations;

namespace SARSTWebApplication.Enums
{
    public enum ResidentPronouns
    {
        [Display(Name = "He/Him/His")]
        hehimhis,
        [Display(Name = "She/Her/Hers")]
        sheherhers,
        [Display(Name = "They/Them/Theirs")]
        theythemtheirs,
    }
}