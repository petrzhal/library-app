using Library.Domain.Models;
using Library.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Tests.Repositories
{
    public abstract class AuthorRepositoryTestBase
    {
        protected LibraryDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new LibraryDbContext(options);
        }
    }
}
