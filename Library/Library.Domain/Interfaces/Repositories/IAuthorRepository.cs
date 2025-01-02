using Library.Domain.Models;

namespace Library.Domain.Interfaces.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<List<Author>> GetByPageAsync(PageInfo pageInfo);
        Task<int> GetCountAsync();
    }
}
