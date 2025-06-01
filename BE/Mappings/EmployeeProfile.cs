using AutoMapper;
using DemoImportExport.DTOs.Employee;
using DemoImportExport.DTOs.Employees;
using DemoImportExport.Models;
using MISA.AMISDemo.Core.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoImportExport.Mapping
{
    public class EmployeeProfile: Profile 
    {
        public EmployeeProfile() {
            CreateMap<Employee,EmployeeDto>();
            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
            CreateMap<EmployeeExcelDto, EmployeeDto>().ReverseMap();
            CreateMap<EmployeeExcelDto, Employee>().ReverseMap();
            CreateMap<Employee, EmployeeImportDto>().ReverseMap();
        }
    }
}
