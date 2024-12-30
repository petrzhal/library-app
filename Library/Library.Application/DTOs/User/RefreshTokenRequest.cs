using MediatR;

namespace Library.Application.DTOs.User
{
    public record RefreshTokenRequest(string RefreshToken) : IRequest<UserAuthResponse>;
}
