using AutoMapper;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Book;
using Library.Domain.Models;

namespace Library.Application.Mappings
{
    public class PageInfoProfile : Profile
    {
        public PageInfoProfile() 
        {
            CreateMap<BookListRequest, PageInfo>()
                .ForMember(dest => dest.PageIndex, opt => opt.MapFrom(src => src.PageIndex))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize));
            CreateMap<AuthorListRequest, PageInfo>()
                .ForMember(dest => dest.PageIndex, opt => opt.MapFrom(src => src.PageIndex))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize));
        }
    }
}
