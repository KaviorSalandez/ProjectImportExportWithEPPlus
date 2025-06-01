using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DemoImportExport.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        public string? EmployeeName { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        [NotMapped]
        public string? DepartmentName { get; set; }

        [Required]
        public int PositionId { get; set; }

        [ForeignKey("PositionId")]
        public Position? Position { get; set; }
        [NotMapped]
        public string? PositionName { get; set; }

        public string? EmployeeCode { get; set; }
        public DateTime? DOB { get; set; }
        public DemoImportExport.Enums.CDKEnum.Gender? Gender { get; set; }
        public string? IDNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? IssuedBy { get; set; }
        public string? Address { get; set; }
        public string? MobilePhone { get; set; }
        public string? LandlinePhone { get; set; }
        public string? Email { get; set; }
        public string? BankAccount { get; set; }
        public string? BankName { get; set; }
        public string? Branch { get; set; }
        public string? Password { get; set; }
        public int TotalRecord { get; set; }
    }
}
