using DemoImportExport.DTOs.Employee;
using DemoImportExport.DTOs.Employees;
using DemoImportExport.Models;
using MISA.AMISDemo.Core.DTOs.Employees;

namespace DemoImportExport.Mappers
{
    public class EmployeeProfile : AutoMapper.Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
            CreateMap<EmployeeExcelDto, EmployeeDto>().ReverseMap();
            CreateMap<EmployeeExcelDto, Employee>().ReverseMap();
            CreateMap<Employee, EmployeeImportDto>().ReverseMap();

            CreateMap<EmployeeImportDto, EmployeeExcelDto>().ReverseMap();
        }
    }
    
}
