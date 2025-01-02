using Library.Domain.Models;

namespace Library.Domain.Interfaces.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetRoleByName(string name);
    }
}
