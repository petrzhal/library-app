using Library.Domain.Models;

namespace Library.Application.Common.Interfaces.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByISBNAsync(string isbn);
        Task<List<Book>> GetByPageAsync(PageInfo pageInfo, string? genre = null, int? authorId = null, string? searchQuery = null);
        Task<int> GetCountAsync(string? genre = null, int? authorId = null, string? searchQuery = null);
        Task<List<Book>> GetByAuthorIdAsync(int authorId);
        Task<List<Book?>> GetUsersBorrowedBooks(int userId);
    }
}
