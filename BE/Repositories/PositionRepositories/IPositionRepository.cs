using DemoImportExport.Models;

namespace DemoImportExport.Repositories.PositionRepositories
{
    public interface IPositionRepository : IGenericRepository<Position>
    {
        Task<IEnumerable<Position>> GetAllAsync();
        Task<Position?> GetByIdAsync(int id);
        Task<Position> CheckPositionName(string PositionName);
    }
}
