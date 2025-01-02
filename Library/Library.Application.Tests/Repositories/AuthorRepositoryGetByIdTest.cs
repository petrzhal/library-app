using Library.Domain.Models;
using Library.Infrastructure.Repositories;

namespace Library.Application.Tests.Repositories
{
    public class AuthorRepositoryGetByIdTest : AuthorRepositoryTestBase
    {
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
