using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoImportExport.DTOs.Employees
{
    public class EmployeeCountDto
    {
        public int Count { get; set; }
        public List<EmployeeDto> Employees { get; set; }

    }
}
