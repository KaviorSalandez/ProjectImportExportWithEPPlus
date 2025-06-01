using DemoImportExport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMISDemo.Core.DTOs.Employees
{
    public class EmployeeImportDto : Employee
    {
        public List<string> Errors { get; set; }  // Ghi nhận các lỗi khi import    
        public bool IsImported { get; set; }    // Đánh dấu xem bản ghi đã hợp lệ hay chưa

        public EmployeeImportDto()
        {
            Errors = new List<string>();
        }
    }
}
