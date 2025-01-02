using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models;
using Library.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class RoleRepository(LibraryDbContext dbContext) : Repository<Role>(dbContext), IRoleRepository
    {
        public async Task<Role> GetRoleByName(string name)
        {
            return await _context.Set<Role>().FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
