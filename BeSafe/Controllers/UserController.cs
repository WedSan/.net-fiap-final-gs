using BeSafe.Data.Repository;
using BeSafe.Models;

namespace BeSafe.Data.Controller;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _userRepository.GetUserWithDetailsAsync(id);
        
        if (user == null)
            return NotFound();
            
        return Ok(user);
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        try
        {
            var createdUser = await _userRepository.AddAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id)
            return BadRequest();
            
        try
        {
            await _userRepository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userRepository.DeleteAsync(id);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}