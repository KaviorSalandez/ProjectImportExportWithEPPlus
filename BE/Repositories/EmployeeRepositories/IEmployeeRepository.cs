using DemoImportExport.Models;

namespace DemoImportExport.Repositories.EmployeeRepositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> CheckEmployeeCode(string employeeCode);
        Task<Employee> CheckEmployeeCCCD(string cccd);
        Task<Employee> CheckBankAccount(string bankAccount);
        int InsertMany(List<Employee> entities);
        public Task<IEnumerable<Employee>> FindAllFilter(int pageSize = 10, int pageNumber = 1, string search = "", string? email = "");
        Task<IEnumerable<Employee>> FindManyRecord(List<int> Ids);
        Task<List<string>> GetExistingEmployeeCodes(List<string> codes);
    }
}
