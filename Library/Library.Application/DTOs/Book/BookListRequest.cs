using MediatR;

namespace Library.Application.DTOs.Book
{
    public record BookListRequest(
        int PageIndex,
        int PageSize,
        string? Genre = null,
        int? AuthorId = null,
        string? Title = null
    ) : IRequest<Pagination<BookDto>>;
}
