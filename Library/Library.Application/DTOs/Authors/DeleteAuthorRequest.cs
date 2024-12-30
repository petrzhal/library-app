using MediatR;

namespace Library.Application.DTOs.Authors
{
    public record DeleteAuthorRequest(int AuthorId) : IRequest<Unit>;
}
