using MediatR;
using Library.Application.UseCases.Authors;
using Library.Domain.Models;
using Library.Application.DTOs.Authors;
using Moq;

namespace Library.Application.Tests.UseCases
{
    public class UpdateAuthorHandlerTests : HandlerTestBase
    {
        [Fact]
        public async Task Handle_ShouldUpdateAuthor()
        {
            // Arrange
            var request = new UpdateAuthorRequest(1, "UpdatedFirstName", "UpdatedLastName", new DateTime(1980, 1, 1), "UpdatedCountry");
            var author = new Author { Id = 1, FirstName = "OriginalFirstName", LastName = "OriginalLastName", Country = "OriginalCountry" };

            MapperMock.Setup(m => m.Map<Author>(request)).Returns(author);
            UnitOfWorkMock.Setup(u => u.Authors.UpdateAsync(author)).Returns(Task.CompletedTask);
            UnitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var handler = new UpdateAuthorHandler(UnitOfWorkMock.Object, MapperMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
        }
    }
}
