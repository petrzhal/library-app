using Library.Domain.Models;
using Library.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Tests.Repositories
{
    public class AuthorRepositoryAddTest : AuthorRepositoryTestBase
    {
        [Fact]
        public async Task AddAsync_ShouldAddAuthorToDatabase()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var repository = new AuthorRepository(context);

            var author = new Author
            {
                FirstName = "Test First name",
                LastName = "Test Last name",
                Country = "Belarus",
                DateOfBirth = new DateTime(1980, 1, 1)
            };

            // Act
            await repository.AddAsync(author);

            // Assert
            var savedAuthor = await context.Authors.FirstOrDefaultAsync(a => a.FirstName == "Test First name");
            Assert.NotNull(savedAuthor);
            Assert.Equal("Test First name", savedAuthor.FirstName);
            Assert.Equal("Test Last name", savedAuthor.LastName);
        }
    }
}
