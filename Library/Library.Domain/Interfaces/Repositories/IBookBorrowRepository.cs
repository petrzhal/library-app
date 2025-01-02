using Library.Domain.Models;

namespace Library.Domain.Interfaces.Repositories
{
    public interface IBookBorrowRepository : IRepository<BookBorrow>
    {
        Task<BookBorrow> GetByUserAndBookIdAsync(int userId, int bookId);
    }
}
