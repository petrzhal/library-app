using MediatR;

namespace Library.Application.DTOs.User
{
    public record LogoutRequest : IRequest<Unit>;
}
