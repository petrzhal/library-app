using Library.Application.UseCases.Authors;
using Library.Domain.Models;
using Library.Application.DTOs.Authors;
using Moq;

namespace Library.Application.Tests.UseCases
{
    public class GetAuthorByIdHandlerTests : HandlerTestBase
    {
        [Fact]
        public async Task Handle_ShouldReturnAuthorDto()
        {
            // Arrange
            var request = new AuthorIdRequest(1);
            var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
            var authorDto = new AuthorDto(1, "John", "Doe", new DateTime(1980, 1, 1), "Belarus");

            UnitOfWorkMock.Setup(u => u.Authors.GetByIdAsync(1)).ReturnsAsync(author);
            MapperMock.Setup(m => m.Map<AuthorDto>(author)).Returns(authorDto);

            var handler = new GetAuthorByIdHandler(UnitOfWorkMock.Object, MapperMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(authorDto, result);
            UnitOfWorkMock.Verify(u => u.Authors.GetByIdAsync(1), Times.Once);
        }
    }
}
