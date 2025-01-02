using FluentValidation;
using Library.Application.Common.Interfaces.Services;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs.User;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Library.Application.DTOs.Book;
using Library.Domain.Models;
using MediatR;

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
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var tokensPair = await _tokenService.GenerateTokensPairAsync(user);

            return _mapper.Map<UserAuthResponse>(tokensPair);
        }
    }
}
