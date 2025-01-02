using Library.Application.UseCases.Authors;
using Library.Domain.Models;
using Library.Application.DTOs.Authors;
using Moq;

namespace Library.Application.Tests.UseCases
{
    public class GetAuthorsListHandlerTests : HandlerTestBase
    {
        [Fact]
        public async Task Handle_ShouldReturnPaginatedAuthors()
        {
            // Arrange
            var request = new AuthorListRequest(1, 2);
            var pageInfo = new PageInfo(1, 2);
            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "Author1", LastName = "LastName1" },
                new Author { Id = 2, FirstName = "Author2", LastName = "LastName2" }
            };
            var authorDtos = new List<AuthorDto>
            {
                new AuthorDto(1, "Author1", "LastName1", new DateTime(1980, 1, 1), "Belarus"),
                new AuthorDto(2, "Author2", "LastName2", new DateTime(1975, 1, 1), "Belarus")
            };

            MapperMock.Setup(m => m.Map<PageInfo>(request)).Returns(pageInfo);
            UnitOfWorkMock.Setup(u => u.Authors.GetByPageAsync(pageInfo)).ReturnsAsync(authors);
            UnitOfWorkMock.Setup(u => u.Authors.GetCountAsync()).ReturnsAsync(10);
            MapperMock.Setup(m => m.Map<List<AuthorDto>>(authors)).Returns(authorDtos);

            var handler = new GetAuthorsListHandler(UnitOfWorkMock.Object, MapperMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(10, result.TotalCount);
        }
    }
}
