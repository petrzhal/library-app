using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Book;
using Library.Domain.Models;
using MediatR;

namespace Library.Application.UseCases.Books
{
    public class GetBooksByAuthorHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AuthorBooksRequest, Pagination<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Pagination<BookDto>> Handle(AuthorBooksRequest request, CancellationToken cancellationToken)
        {
            var authors = await _unitOfWork.Books.GetByAuthorIdAsync(request.AuthorId);
            var books = _mapper.Map<List<BookDto>>(authors);
            return Pagination<BookDto>.ToPagedList(books, _mapper.Map<PageInfo>(request));
        }

    }
}
