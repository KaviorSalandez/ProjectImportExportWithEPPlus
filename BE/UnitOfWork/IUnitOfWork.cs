using DemoImportExport.Repositories.DepartmentRepositories;
using DemoImportExport.Repositories.EmployeeRepositories;
using DemoImportExport.Repositories.PositionRepositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DemoImportExport.Uow
{
    public interface IUnitOfWork
    {
        public IDepartmentRepository DepartmentRepository { get; }
        public IPositionRepository PositionRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        Task<bool> SaveChangeAsync();
        IExecutionStrategy CreateExecutionStrategy();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
