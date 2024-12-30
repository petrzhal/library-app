using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.Common.Interfaces.Services;
using Library.Application.DTOs.User;
using MediatR;

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
            return _mapper.Map<UserDto>(await _unitOfWork.Users.GetByIdAsync(userId));
        }
    }
}
