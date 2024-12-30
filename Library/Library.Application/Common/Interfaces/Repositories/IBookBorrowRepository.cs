using Library.Domain.Models;

namespace Library.Application.Common.Interfaces.Repositories
{
    public interface IBookBorrowRepository : IRepository<BookBorrow>
    {
        Task<BookBorrow> GetByUserAndBookIdAsync(int userId, int bookId);
    }
}
