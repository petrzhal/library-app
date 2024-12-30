using MediatR;

namespace Library.Application.DTOs.Book
{    public record ReturnBookRequest(int BookId) : IRequest<Unit>;
}
