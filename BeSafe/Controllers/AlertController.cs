using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeSafe.Data.Repository;
using BeSafe.Models;
using BeSafe.Services;

[ApiController]
[Route("api/[controller]")]
public class AlertController : ControllerBase
{
      private readonly IAlertRepository _alertRepository;
    private readonly RabbitMQService _rabbitMQService;
    private readonly ILogger<AlertController> _logger;
    
    public AlertController(
        IAlertRepository alertRepository,
        RabbitMQService rabbitMQService,
        ILogger<AlertController> logger)
    {
        _alertRepository = alertRepository;
        _rabbitMQService = rabbitMQService;
        _logger = logger;
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
            alert.DataEnvio = DateTime.UtcNow; // Garantir data atual
            var createdAlert = await _alertRepository.AddAsync(alert);
            
            var alertMessage = new AlertMessageDto
            {
                Id = createdAlert.Id,
                AreaRiscoId = createdAlert.AreaRiscoId,
                Mensagem = createdAlert.Mensagem,
                DataEnvio = createdAlert.DataEnvio,
                TipoAlerta = createdAlert.TipoAlerta,
                Timestamp = DateTime.Now,
                Username = "System"
            };
            
            _rabbitMQService.PublishAlert(alertMessage, RabbitMQService.NewAlertRoutingKey);
            _logger.LogInformation($"Alert created and published: ID {createdAlert.Id}");
            
            return CreatedAtAction(nameof(GetAlert), new { id = createdAlert.Id }, createdAlert);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating alert");
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
            
            var alertMessage = new AlertMessageDto
            {
                Id = alert.Id,
                AreaRiscoId = alert.AreaRiscoId,
                Mensagem = alert.Mensagem,
                DataEnvio = alert.DataEnvio,
                TipoAlerta = alert.TipoAlerta,
                Timestamp = DateTime.Now,
                Username = "System"
            };
            
            _rabbitMQService.PublishAlert(alertMessage, RabbitMQService.UpdatedAlertRoutingKey);
            _logger.LogInformation($"Alert updated and published: ID {alert.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating alert");
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