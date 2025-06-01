using MISA.AMISDemo.Core.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoImportExport.DTOs.Employees
{
    public class EmployeeImportParentDto
    {

        public int CountSuccess { get; set; }

        public int CountFail { get; set; }

        public string KeyRedisImportSuccess { get; set; }

        public string KeyRedisImportFail { get; set; }  // Fail khi CountFail > 0

        public List<EmployeeImportDto> EmployeeImportDtos { get; set; }

        public EmployeeImportParentDto()
        {
            EmployeeImportDtos = new List<EmployeeImportDto>();
        }
    }
}
