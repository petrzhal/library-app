using MediatR;

namespace Library.Application.DTOs.Authors
{
    public record AuthorIdRequest(int AuthorId) : IRequest<AuthorDto>;
}
