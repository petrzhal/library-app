using Library.Domain.Models;

namespace Library.Application.Common.Interfaces.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetRoleByName(string name);
    }
}
