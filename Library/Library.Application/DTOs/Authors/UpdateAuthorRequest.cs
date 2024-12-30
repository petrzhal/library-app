using MediatR;

namespace Library.Application.DTOs.Authors
{
    public record UpdateAuthorRequest(
        int Id,
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        string Country
    ) : IRequest<Unit>;
}
