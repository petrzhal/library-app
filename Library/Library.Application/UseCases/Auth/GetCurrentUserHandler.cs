using AutoMapper;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Interfaces.Services;
using Library.Application.DTOs.User;
using MediatR;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Auth
{
    public class GetCurrentUserHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService) : IRequestHandler<GetCurrentUserRequest, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenService _tokenService = tokenService;
        public async Task<UserDto> Handle(GetCurrentUserRequest _, CancellationToken cancellationToken)
        {
            var userId = _tokenService.GetUserIdFromAccessToken();
            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UserDto>(user);
        }
    }
}
