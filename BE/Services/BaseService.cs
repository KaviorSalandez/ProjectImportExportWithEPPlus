using DemoImportExport.Uow;

namespace DemoImportExport.Services
{
    public class BaseService
    {
        protected IUnitOfWork UnitOfWork { get; set; }
        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
