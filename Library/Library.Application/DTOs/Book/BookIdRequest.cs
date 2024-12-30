using MediatR;

namespace Library.Application.DTOs.Book
{
    public record BookIdRequest(int BookId) : IRequest<BookDto>;
}
