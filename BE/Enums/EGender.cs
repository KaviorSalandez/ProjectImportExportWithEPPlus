using System.ComponentModel.DataAnnotations;

namespace DemoImportExport.Enums
{
    public enum EGender
    {
        [Display(Name = "Nam")]
        Male = 1,
        [Display(Name = "Nữ")]
        Female = 2,
        [Display(Name = "Khác")]
        Other = 3,
    }
}
