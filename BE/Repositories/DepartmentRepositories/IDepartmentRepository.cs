using DemoImportExport.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoImportExport.Repositories.DepartmentRepositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<Department> CheckDepartmentName(string departmentName);
    }
}
