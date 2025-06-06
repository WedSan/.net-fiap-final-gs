using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeSafe.Data.Repository;
using BeSafe.Models;

[ApiController]
[Route("api/[controller]")]
public class AlertController : ControllerBase
{
    private readonly IAlertRepository _alertRepository;
    
    public AlertController(IAlertRepository alertRepository)
    {
        _alertRepository = alertRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Alert>>> GetAlerts()
    {
        var alerts = await _alertRepository.GetAllAsync();
        return Ok(alerts);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Alert>> GetAlert(int id)
    {
        var alert = await _alertRepository.GetByIdAsync(id);
        
        if (alert == null)
            return NotFound();
            
        return Ok(alert);
    }
    
    [HttpGet("riskarea/{riskAreaId}")]
    public async Task<ActionResult<IEnumerable<Alert>>> GetAlertsByRiskArea(int riskAreaId)
    {
        var alerts = await _alertRepository.GetAlertsByRiskAreaAsync(riskAreaId);
        return Ok(alerts);
    }
    
    [HttpPost]
    public async Task<ActionResult<Alert>> CreateAlert(Alert alert)
    {
        try
        {
            var createdAlert = await _alertRepository.AddAsync(alert);
            return CreatedAtAction(nameof(GetAlert), new { id = createdAlert.Id }, createdAlert);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAlert(int id, Alert alert)
    {
        if (id != alert.Id)
            return BadRequest();
            
        try
        {
            await _alertRepository.UpdateAsync(alert);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlert(int id)
    {
        var result = await _alertRepository.DeleteAsync(id);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}