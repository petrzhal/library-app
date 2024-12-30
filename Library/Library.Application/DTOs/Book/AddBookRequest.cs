using MediatR;

namespace Library.Application.DTOs.Book
{
    public record AddBookRequest(
        string ISBN,
        string Title,
        string Genre,
        string Description,
        int AuthorId,
        string ImageData,
        string ImageType
    ) : IRequest<Unit>;
}
