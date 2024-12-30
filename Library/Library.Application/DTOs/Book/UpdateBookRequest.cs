using MediatR;

namespace Library.Application.DTOs.Book
{
    public record UpdateBookRequest(
        int Id,
        string ISBN,
        string Title,
        string Genre,
        string Description,
        int AuthorId,
        string ImageId,
        string ImageData,
        string ImageType
    ) : IRequest<Unit>;
}
