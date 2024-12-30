using MediatR;

namespace Library.Application.DTOs.User
{
    public record UserRegisterRequest(
        string Username,
        string Email,
        string Password
    ) : IRequest<UserAuthResponse>;
}
