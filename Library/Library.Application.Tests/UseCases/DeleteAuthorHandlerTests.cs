using MediatR;
using Library.Application.UseCases.Authors;
using Library.Domain.Models;
using Library.Application.DTOs.Authors;
using Moq;

namespace Library.Application.Tests.UseCases
{
    public class DeleteAuthorHandlerTests : HandlerTestBase
    {
        [Fact]
        public async Task Handle_ShouldDeleteAuthor()
        {
            // Arrange
            var request = new DeleteAuthorRequest(1);
            var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };

            UnitOfWorkMock.Setup(u => u.Authors.GetByIdAsync(1)).ReturnsAsync(author);
            UnitOfWorkMock.Setup(u => u.Authors.DeleteAsync(author)).Returns(Task.CompletedTask);
            UnitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var handler = new DeleteAuthorHandler(UnitOfWorkMock.Object, MapperMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            UnitOfWorkMock.Verify(u => u.Authors.GetByIdAsync(1), Times.Once);
            UnitOfWorkMock.Verify(u => u.Authors.DeleteAsync(author), Times.Once);
            UnitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}
