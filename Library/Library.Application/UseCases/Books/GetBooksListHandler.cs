using Library.Application.Common.Interfaces;
using Library.Application.DTOs.Book;
using Library.Application.DTOs;
using Library.Domain.Models;
using MediatR;
using AutoMapper;

namespace Library.Application.UseCases.Books
{
    public class GetBooksListHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<BookListRequest, Pagination<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Pagination<BookDto>> Handle(BookListRequest request, CancellationToken cancellationToken)
        {
            var pageInfo = _mapper.Map<PageInfo>(request);
            
            var books = await _unitOfWork.Books.GetByPageAsync(pageInfo, request.Genre, request.AuthorId, request.Title);
            var totalCount = await _unitOfWork.Books.GetCountAsync(request.Genre, request.AuthorId, request.Title);

            var bookDtos = _mapper.Map<List<BookDto>>(books);
            return new Pagination<BookDto>(bookDtos, totalCount, pageInfo);
        }
    }
}
