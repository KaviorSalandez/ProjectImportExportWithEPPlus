using DemoImportExport.Models;
using DemoImportExport.Persistents;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System.Collections.Concurrent;

namespace DemoImportExport.Repositories.EmployeeRepositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> CheckEmployeeCode(string employeeCode)
        {
            var entity = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
            return entity;
        }

        public async Task<Employee> CheckBankAccount(string bankAccount)
        {
            var entity = await _context.Employees.FirstOrDefaultAsync(e => e.BankAccount != null && e.BankAccount.Trim() == bankAccount.Trim());
            return entity;
        }

        public int InsertMany(List<Employee> employees)
        {
            if (employees == null || !employees.Any())
                return 0;

            _context.BulkInsert(employees);
            return employees.Count();
        }
        public async Task<IEnumerable<Employee>> FindAllFilter(int pageSize = 10, int pageNumber = 1, string search = "", string? email = "")
        {
            var query = _context.Employees.AsQueryable();

            // Nếu có email, lọc theo Email
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(e => e.Email == email);
            }

            // Nếu có tìm kiếm theo tên
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.EmployeeName.Contains(search));
            }

            // Phân trang
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Employee>> FindManyRecord(List<int> Ids)
        {

            var employees = await _context.Employees
                .Where(e => Ids.Contains(e.EmployeeId))
                .Include(e => e.Department)
                .Include(e => e.Position)
                .ToListAsync();

            return employees;
        }

        public async Task<List<string>> GetExistingEmployeeCodes(List<string> codes)
        {
            return await _context.Employees
                            .Where(e => codes.Contains(e.EmployeeCode))
                            .Select(e => e.EmployeeCode)
                            .ToListAsync();
        }

        public async Task<Employee> CheckEmployeeCCCD(string cccd)
        {
            var entity = await _context.Employees.FirstOrDefaultAsync(e => e.IDNo == cccd);
            return entity;
        }
    }
}
