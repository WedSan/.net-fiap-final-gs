using BeSafe.Models;
using Microsoft.EntityFrameworkCore;

namespace BeSafe.Data.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    // Get user with contact and address
    public async Task<User> GetUserWithDetailsAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Contact)
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserWithDetailsAsync(int id);
}