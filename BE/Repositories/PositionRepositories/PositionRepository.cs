using DemoImportExport.Models;
using DemoImportExport.Persistents;
using Microsoft.EntityFrameworkCore;

namespace DemoImportExport.Repositories.PositionRepositories
{
    public class PositionRepository : GenericRepository<Position>, IPositionRepository
    {
        public PositionRepository(AppDbContext context): base(context)
        {
        }
        public async Task<IEnumerable<Position>> GetAllAsync()
        {
            return await _context.Positions.ToListAsync();
        }

        public async Task<Position?> GetByIdAsync(int id)
        {
            return await _context.Positions.FindAsync(id);
        }
        public async Task<Position> CheckPositionName(string positionName)
        {
            return await _context.Positions
                .FirstOrDefaultAsync(p => p.PositionName.Trim() == positionName.Trim());
        }

    }
}
