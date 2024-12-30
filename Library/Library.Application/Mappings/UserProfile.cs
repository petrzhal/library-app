using AutoMapper;
using Library.Application.DTOs.User;
using Library.Domain.Models;

namespace Library.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserAuthResponse>();
            CreateMap<UserRegisterRequest, User>();
            CreateMap<UserAuthResponse, TokensPair>();
            CreateMap<TokensPair, UserAuthResponse>();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
        }
    }
}
