using AutoMapper;
using Library.Application.DTOs.Book;
using Library.Application.DTOs.Images;
using Library.Domain.Models;

namespace Library.Application.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<AddBookRequest, Book>()
                .ForMember(dest => dest.BorrowedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ReturnBy, opt => opt.Ignore());

            CreateMap<UpdateBookRequest, Book>()
                .ForMember(dest => dest.BorrowedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ReturnBy, opt => opt.Ignore());

            CreateMap<Image, AddBookRequest>().ReverseMap();
            CreateMap<Image, UpdateBookRequest>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.ImageData, opt => opt.Condition(src => src.ImageData != null))
                .ForMember(dest => dest.ImageType, opt => opt.Condition(src => src.ImageType != null));


            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author));

            CreateMap<BorrowBookRequest, BookBorrow>();
            CreateMap<GetUsersBorrowedBooksRequest, PageInfo>();
            CreateMap<Image, ImageDto>().ReverseMap();
        }
    }
}
