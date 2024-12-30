using Library.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Persistence;
using Library.Domain.Models;

namespace Library.Infrastructure.Repositories
{
    public class BookRepository(LibraryDbContext context) : Repository<Book>(context), IBookRepository
    {
        public new async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _context.Set<Book>()
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<List<Book>> GetByPageAsync(PageInfo pageInfo, string? genre = null, int? authorId = null, string? searchQuery = null)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .AsNoTracking();

            query = query.Where(b => (b.BorrowedAt == null) && (b.ReturnBy == null));

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(b => b.Genre == genre);

            if (authorId.HasValue)
                query = query.Where(b => b.AuthorId == authorId.Value);

            if (!string.IsNullOrEmpty(searchQuery))
                query = query.Where(b => b.Title.Contains(searchQuery));

            return await query
                .Skip((pageInfo.PageIndex - 1) * pageInfo.PageSize)
                .Take(pageInfo.PageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync(string? genre = null, int? authorId = null, string? title = null)
        {
            var query = _context.Books.AsQueryable();

            query = query.Where(b => (b.BorrowedAt == null) && (b.ReturnBy == null));

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(b => b.Genre == genre);

            if (authorId.HasValue)
                query = query.Where(b => b.AuthorId == authorId.Value);

            if (!string.IsNullOrEmpty(title))
                query = query.Where(b => b.Title.Contains(title));

            return await query.CountAsync();
        }

        public async Task<List<Book>> GetByAuthorIdAsync(int authorId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<List<Book?>> GetUsersBorrowedBooks(int userId)
        {
            return await _context.BookBorrows
                .Where(bb => bb.UserId == userId)
                .Include(bb => bb.Book)
                .ThenInclude(b => b.Author)
                .Select(bb => bb.Book)
                .ToListAsync();
        }
    }
}
