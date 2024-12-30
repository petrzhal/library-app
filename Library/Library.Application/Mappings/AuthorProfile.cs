using AutoMapper;
using Library.Application.DTOs.Authors;
using Library.Domain.Models;

namespace Library.Application.Mappings
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorDto, Author>().ReverseMap();
            CreateMap<AddAuthorRequest, Author>().ReverseMap();
            CreateMap<UpdateAuthorRequest, Author>().ReverseMap();
            CreateMap<DeleteAuthorRequest, Author>().ReverseMap();
            CreateMap<AuthorBooksRequest, PageInfo>();
        }
    }
}
