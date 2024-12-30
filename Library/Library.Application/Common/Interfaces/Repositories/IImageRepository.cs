using Library.Domain.Models;

namespace Library.Application.Common.Interfaces.Repositories
{
    public interface IImageRepository : IRepository<Image>
    {
        new Task<string> AddAsync(Image entity);
    }
}
