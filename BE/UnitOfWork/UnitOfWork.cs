using DemoImportExport.Persistents;
using DemoImportExport.Repositories.DepartmentRepositories;
using DemoImportExport.Repositories.EmployeeRepositories;
using DemoImportExport.Repositories.PositionRepositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DemoImportExport.Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private DepartmentRepository _departmentRepository;
        private PositionRepository _positionRepository;
        private EmployeeRepository _employeeRepository;
        public UnitOfWork(AppDbContext context) => _context = context;
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task<bool> SaveChangeAsync()
        {
            var cm = await _context.SaveChangesAsync().ConfigureAwait(false);
            return cm != 0;
        }
        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _context.Database.CreateExecutionStrategy();
        }
        /// <summary>
        /// free memory cache and close connection
        /// </summary>
        private bool disposed = false;

        public IDepartmentRepository DepartmentRepository
        {
            get { return _departmentRepository ?? (_departmentRepository = new Repositories.DepartmentRepositories.DepartmentRepository(_context)); }
        }

        public IPositionRepository PositionRepository
        {
            get { return _positionRepository ?? (_positionRepository = new Repositories.PositionRepositories.PositionRepository(_context)); }
        }

        public IEmployeeRepository EmployeeRepository
        {
            get { return _employeeRepository ?? (_employeeRepository = new Repositories.EmployeeRepositories.EmployeeRepository(_context)); }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
