using MediatR;

namespace Library.Application.DTOs.Book
{
    public record GetUsersBorrowedBooksRequest(int PageIndex, int PageSize) : IRequest<Pagination<BookDto>>;
}
