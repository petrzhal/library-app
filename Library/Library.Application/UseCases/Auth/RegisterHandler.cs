using Library.Application.Common.Interfaces.Services;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs.User;
using Library.Domain.Models;
using AutoMapper;
using MediatR;

namespace Library.Application.UseCases.Auth
{
    public class RegisterHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IPasswordHasher passwordHasher,
        IMapper mapper) : IRequestHandler<UserRegisterRequest, UserAuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IMapper _mapper = mapper;

        public async Task<UserAuthResponse> Handle(UserRegisterRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.Users.GetByUserNameAsync(request.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists.");
            }

            var role = await _unitOfWork.Roles.GetRoleByName("User");

            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                Password = hashedPassword,
                Email = request.Email,
                RoleId = role.Id
            };

            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.CompleteAsync();

            var tokensPair = await _tokenService.GenerateTokensPairAsync(newUser);

            return _mapper.Map<UserAuthResponse>(tokensPair);
        }
    }
}
