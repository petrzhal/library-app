using Library.Domain.Models;
using Library.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Persistence;

namespace Library.Infrastructure.Repositories
{
    public class UserRepository(LibraryDbContext context) : Repository<User>(context), IUserRepository
    {
        public new async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Set<User>()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Username == userName);
        }
    }
}
