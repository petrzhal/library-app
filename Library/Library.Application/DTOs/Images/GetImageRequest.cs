using MediatR;

namespace Library.Application.DTOs.Images
{
    public record GetImageRequest(int BookId) : IRequest<ImageDto>;
}
