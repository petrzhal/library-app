using MediatR;

namespace Library.Application.DTOs.Book
{
    public record BorrowBookRequest(int BookId) : IRequest<Unit>;
}
