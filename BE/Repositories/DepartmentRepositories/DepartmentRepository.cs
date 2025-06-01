using DemoImportExport.Models;
using DemoImportExport.Persistents;
using Microsoft.EntityFrameworkCore;

namespace DemoImportExport.Repositories.DepartmentRepositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }
        
        public async Task<Department> CheckDepartmentName(string departmentName)
        {
            var findName = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName.Trim() == departmentName.Trim());

            return findName;
        }
    }
}
