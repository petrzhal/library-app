using MediatR;
using Library.Application.UseCases.Authors;
using Library.Domain.Models;
using Library.Application.DTOs.Authors;
using Moq;

namespace Library.Application.Tests.UseCases
{
    public class AddAuthorHandlerTests : HandlerTestBase
    {
        [Fact]
        public async Task Handle_ShouldAddAuthor()
        {
            // Arrange
            var request = new AddAuthorRequest("John", "Doe", new DateTime(1980, 1, 1), "USA");
            var author = new Author { FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1980, 1, 1), Country = "USA" };

            MapperMock.Setup(m => m.Map<Author>(request)).Returns(author);
            UnitOfWorkMock.Setup(u => u.Authors.AddAsync(author)).Returns(Task.CompletedTask);
            UnitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var handler = new AddAuthorHandler(UnitOfWorkMock.Object, MapperMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            UnitOfWorkMock.Verify(u => u.Authors.AddAsync(author), Times.Once);
            UnitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}
