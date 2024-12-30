using Library.Domain.Models;

namespace Library.Application.Common.Interfaces.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<List<Author>> GetByPageAsync(PageInfo pageInfo);
        Task<int> GetCountAsync();
    }
}
