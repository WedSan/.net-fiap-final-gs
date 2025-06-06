using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeSafe.Data.Repository;
using BeSafe.Models;

[ApiController]
[Route("api/[controller]")]
public class RiskAreaController : ControllerBase
{
    private readonly IRiskAreaRepository _riskAreaRepository;
    
    public RiskAreaController(IRiskAreaRepository riskAreaRepository)
    {
        _riskAreaRepository = riskAreaRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RiskArea>>> GetRiskAreas()
    {
        var riskAreas = await _riskAreaRepository.GetAllAsync();
        return Ok(riskAreas);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<RiskArea>> GetRiskArea(int id)
    {
        var riskArea = await _riskAreaRepository.GetRiskAreaWithAlertsAsync(id);
        
        if (riskArea == null)
            return NotFound();
            
        return Ok(riskArea);
    }
    
    [HttpPost]
    public async Task<ActionResult<RiskArea>> CreateRiskArea(RiskArea riskArea)
    {
        try
        {
            var createdRiskArea = await _riskAreaRepository.AddAsync(riskArea);
            return CreatedAtAction(nameof(GetRiskArea), new { id = createdRiskArea.Id }, createdRiskArea);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRiskArea(int id, RiskArea riskArea)
    {
        if (id != riskArea.Id)
            return BadRequest();
            
        try
        {
            await _riskAreaRepository.UpdateAsync(riskArea);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRiskArea(int id)
    {
        var result = await _riskAreaRepository.DeleteAsync(id);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}