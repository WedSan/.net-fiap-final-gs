using BeSafe.Data;
using Microsoft.EntityFrameworkCore;

namespace BeSafe.Services;

public class AlertAnomalyService
{
    private readonly ApplicationDbContext _context;
    private readonly AlertAnomalyPredictor _predictor;

    public AlertAnomalyService(ApplicationDbContext context)
    {
        _context = context;
        _predictor = new AlertAnomalyPredictor();
    }

    public async Task<List<AlertAnomalyPrediction>> DetectAnomaliesAsync()
    {
        var alerts = await _context.Alerts.ToListAsync();
        return _predictor.GetAnomalies(alerts);
    }
}