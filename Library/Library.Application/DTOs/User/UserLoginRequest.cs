using MediatR;

namespace Library.Application.DTOs.User
{
    public record UserLoginRequest(
        string Username,
        string Password
    ) : IRequest<UserAuthResponse>;
}
