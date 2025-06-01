using System.ComponentModel.DataAnnotations;

namespace DemoImportExport.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required, MinLength(5), MaxLength(200)]
        public string DepartmentName { get; set; }

        // Navigation
        public ICollection<Employee> Employees { get; set; }
    }
}
