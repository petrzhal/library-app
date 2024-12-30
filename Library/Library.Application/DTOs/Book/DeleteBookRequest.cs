using MediatR;

namespace Library.Application.DTOs.Book
{
    public record DeleteBookRequest(int BookId) : IRequest<Unit>;
}
