using DemoImportExport.Uow;

namespace DemoImportExport.Services.DepartmentServices
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        public DepartmentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
