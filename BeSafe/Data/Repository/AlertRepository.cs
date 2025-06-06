using BeSafe.Models;
using Microsoft.EntityFrameworkCore;

namespace BeSafe.Data.Repository;

public class AlertRepository : Repository<Alert>, IAlertRepository
{
    public AlertRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Alert>> GetAlertsByRiskAreaAsync(int riskAreaId)
    {
        return await _context.Alerts
            .Where(a => a.AreaRiscoId == riskAreaId)
            .ToListAsync();
    }
}

public interface IAlertRepository : IRepository<Alert>
{
    Task<IEnumerable<Alert>> GetAlertsByRiskAreaAsync(int riskAreaId);
}