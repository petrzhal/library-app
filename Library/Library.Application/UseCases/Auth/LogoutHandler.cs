using Library.Application.Common.Interfaces.Services;
using Library.Application.DTOs.User;
using MediatR;

namespace Library.Application.UseCases.Auth
{
    public class LogoutHandler(ITokenService tokenService) : IRequestHandler<LogoutRequest, Unit>
    {
        private readonly ITokenService _tokenService = tokenService;
        public async Task<Unit> Handle(LogoutRequest _, CancellationToken cancellationToken)
        {
            await _tokenService.RevokeRefreshTokenAsync();
            return Unit.Value;
        }
    }
}
