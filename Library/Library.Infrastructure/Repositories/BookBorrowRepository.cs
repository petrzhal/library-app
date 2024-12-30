using Library.Application.Common.Interfaces.Repositories;
using Library.Domain.Models;
using Library.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class BookBorrowRepository(LibraryDbContext context) : Repository<BookBorrow>(context), IBookBorrowRepository
    {
        public async Task<BookBorrow> GetByUserAndBookIdAsync(int userId, int bookId)
        {
            return await _context.BookBorrows
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == bookId);
        }
    }
}
