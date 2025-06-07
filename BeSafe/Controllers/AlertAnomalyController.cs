using BeSafe.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeSafe.Data.Controller;

[ApiController]
[Route("api/machine_learning")]
public class AlertAnomalyController : ControllerBase
{
    private readonly AlertAnomalyService _anomalyService;

    public AlertAnomalyController(AlertAnomalyService anomalyService)
    {
        _anomalyService = anomalyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAnomalies()
    {
        var result = await _anomalyService.DetectAnomaliesAsync();
        return Ok(result);
    }
}