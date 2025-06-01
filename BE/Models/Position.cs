using System.ComponentModel.DataAnnotations;

namespace DemoImportExport.Models
{
    public class Position
    {
        [Key]
        public int PositionId { get; set; }

        [Required, MinLength(5), MaxLength(200)]
        public string PositionName { get; set; }

        // Navigation
        public ICollection<Employee> Employees { get; set; }
    }
}
