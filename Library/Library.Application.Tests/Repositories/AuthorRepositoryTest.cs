using Library.Domain.Models;
using Library.Infrastructure.Persistence;
using Library.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Tests.Repositories
{
    
    public class AuthorRepositoryTest
    {
        private LibraryDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new LibraryDbContext(options);
        }

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

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuthorById()
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
            context.Authors.Add(author);
            await context.SaveChangesAsync();

            // Act
            var fetchedAuthor = await repository.GetByIdAsync(author.Id);

            // Assert
            Assert.NotNull(fetchedAuthor);
            Assert.Equal(author.FirstName, fetchedAuthor.FirstName);
            Assert.Equal(author.LastName, fetchedAuthor.LastName);
        }
    }

}
