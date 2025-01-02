using Library.Domain.Interfaces.Repositories;
using Library.Domain.Interfaces.Services;
using Library.Application.DTOs.User;
using AutoMapper;
using MediatR;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Auth
{
    public class LoginHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IPasswordHasher passwordHasher,
        IMapper mapper) : IRequestHandler<UserLoginRequest, UserAuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IMapper _mapper = mapper;

        public async Task<UserAuthResponse> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(request.Username);
            if (user == null || !_passwordHasher.VerifyPassword(user.Password, request.Password))
            {
                throw new InvalidCredentialsException();
            }

            var tokensPair = await _tokenService.GenerateTokensPairAsync(user);

            return _mapper.Map<UserAuthResponse>(tokensPair);
        }
    }
}
