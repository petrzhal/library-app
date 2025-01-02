using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models;
using Library.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class AuthorRepository(LibraryDbContext context) : Repository<Author>(context), IAuthorRepository
    {
        public async Task<List<Author>> GetByPageAsync(PageInfo pageInfo)
        {
            return await _context
                .Authors.AsNoTracking()
                .Skip((pageInfo.PageIndex - 1) * pageInfo.PageSize)
                .Take(pageInfo.PageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Authors.CountAsync();
        }
    }
}
