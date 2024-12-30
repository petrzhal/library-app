using MediatR;

namespace Library.Application.DTOs.User
{
    public record GetCurrentUserRequest() : IRequest<UserDto>;
}