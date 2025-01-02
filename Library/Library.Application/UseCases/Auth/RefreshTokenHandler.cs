using AutoMapper;
using Library.Domain.Interfaces.Services;
using Library.Application.DTOs.User;
using MediatR;

namespace Library.Application.UseCases.Auth
{
    public class RefreshTokenHandler(ITokenService tokenService, IMapper mapper) : IRequestHandler<RefreshTokenRequest, UserAuthResponse>
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        public async Task<UserAuthResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var tokensPair = await _tokenService.RefreshTokensAsync(request.RefreshToken);
            return _mapper.Map<UserAuthResponse>(tokensPair);
        }
    }
}
