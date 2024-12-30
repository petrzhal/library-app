using MediatR;

namespace Library.Application.DTOs.Authors
{
    public record AddAuthorRequest(
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        string Country
    ) : IRequest<Unit>;
}
