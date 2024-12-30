using MediatR;

namespace Library.Application.DTOs.Book
{
    public record BookIsbnRequest(string Isbn) : IRequest<BookDto>;
}
