using Library.Application.UseCases.Books;

namespace Library.Application.Tests.UseCases
{
    using AutoMapper;
    using Library.Application.Common.Interfaces;
    using Library.Application.DTOs.Authors;
    using Library.Application.UseCases.Authors;
    using Library.Domain.Models;
    using MediatR;
    using Moq;
    using Xunit;

    public class AuthorHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        public AuthorHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task AddBookHandler_ShouldAddAuthor()
        {
            // Arrange
            var request = new AddAuthorRequest
            (
                "John",
                "Doe",
                new DateTime(1980, 1, 1),
                "USA"
            );

            var author = new Author
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1),
                Country = "USA"
            };

            _mapperMock.Setup(m => m.Map<Author>(It.IsAny<AddAuthorRequest>())).Returns(author);
            _unitOfWorkMock.Setup(u => u.Authors.AddAsync(It.IsAny<Author>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Authors.GetCountAsync()).ReturnsAsync(1);

            var handler = new AddBookHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _unitOfWorkMock.Verify(u => u.Authors.AddAsync(It.IsAny<Author>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthorHandler_ShouldDeleteAuthor()
        {
            // Arrange
            var request = new DeleteAuthorRequest(1);
            var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };

            _unitOfWorkMock.Setup(u => u.Authors.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(author);
            _unitOfWorkMock.Setup(u => u.Authors.DeleteAsync(It.IsAny<Author>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(2);

            var handler = new DeleteAuthorHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _unitOfWorkMock.Verify(u => u.Authors.GetByIdAsync(It.Is<int>(id => id == 1)), Times.Once);
            _unitOfWorkMock.Verify(u => u.Authors.DeleteAsync(It.Is<Author>(a => a.Id == 1)), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAuthorByIdHandler_ShouldReturnAuthorDto()
        {
            // Arrange
            var request = new AuthorIdRequest(1);
            var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
            var authorDto = new AuthorDto(1, "John", "Doe", new DateTime(1980, 1, 1), "Belarus");

            _unitOfWorkMock.Setup(u => u.Authors.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(author);
            _mapperMock.Setup(m => m.Map<AuthorDto>(It.IsAny<Author>())).Returns(authorDto);

            var handler = new GetAuthorByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(authorDto, result);
            _unitOfWorkMock.Verify(u => u.Authors.GetByIdAsync(It.Is<int>(id => id == 1)), Times.Once);
        }

        [Fact]
        public async Task GetAuthorsListHandler_ShouldReturnPaginatedAuthors()
        {
            // Arrange
            var request = new AuthorListRequest(1,2);
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

            _mapperMock.Setup(m => m.Map<PageInfo>(It.IsAny<AuthorListRequest>())).Returns(pageInfo);
            _unitOfWorkMock.Setup(u => u.Authors.GetByPageAsync(It.IsAny<PageInfo>())).ReturnsAsync(authors);
            _unitOfWorkMock.Setup(u => u.Authors.GetCountAsync()).ReturnsAsync(10);
            _mapperMock.Setup(m => m.Map<List<AuthorDto>>(It.IsAny<List<Author>>())).Returns(authorDtos);

            var handler = new GetAuthorsListHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(10, result.TotalCount);
            _unitOfWorkMock.Verify(u => u.Authors.GetByPageAsync(It.Is<PageInfo>(p => p.PageIndex == 1 && p.PageSize == 2)), Times.Once);
            _unitOfWorkMock.Verify(u => u.Authors.GetCountAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAuthorHandler_ShouldUpdateAuthor()
        {
            // Arrange
            var request = new UpdateAuthorRequest
            (
                1,
                "UpdatedFirstName",
                "UpdatedLastName",
                new DateTime(1980, 1, 1),
                "UpdatedCountry"
            );

            var author = new Author
            {
                Id = 1,
                FirstName = "OriginalFirstName",
                LastName = "OriginalLastName",
                Country = "OriginalCountry"
            };

            _mapperMock.Setup(m => m.Map<Author>(It.IsAny<UpdateAuthorRequest>())).Returns(author);
            _unitOfWorkMock.Setup(u => u.Authors.UpdateAsync(It.IsAny<Author>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var handler = new UpdateAuthorHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _unitOfWorkMock.Verify(u => u.Authors.UpdateAsync(It.IsAny<Author>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }

}
