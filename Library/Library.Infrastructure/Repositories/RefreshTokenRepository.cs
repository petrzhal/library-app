using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models;
using Library.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class RefreshTokenRepository(LibraryDbContext context) : Repository<RefreshToken>(context), IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.Set<RefreshToken>()
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }
        public async Task<IEnumerable<RefreshToken>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Set<RefreshToken>()
                .Where(rt => rt.UserId == userId)
                .ToListAsync();
        }
    }
}
