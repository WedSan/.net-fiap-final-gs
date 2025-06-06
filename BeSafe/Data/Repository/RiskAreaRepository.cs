using BeSafe.Models;
using Microsoft.EntityFrameworkCore;

namespace BeSafe.Data.Repository;

public class RiskAreaRepository : Repository<RiskArea>, IRiskAreaRepository
{
    public RiskAreaRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<RiskArea> GetRiskAreaWithAlertsAsync(int id)
    {
        return await _context.RiskAreas
            .Include(r => r.Alerts)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}

public interface IRiskAreaRepository : IRepository<RiskArea>
{
    Task<RiskArea> GetRiskAreaWithAlertsAsync(int id);
}