using Library.Application.DTOs.Book;
using MediatR;

namespace Library.Application.DTOs.Authors
{
    public record AuthorBooksRequest(
        int AuthorId,
        int PageIndex,
        int PageSize        
    ) : IRequest<Pagination<BookDto>>;
}
