using AutoMapper;
using Library.Domain.Interfaces.Repositories;
using Library.Application.DTOs;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Book;
using Library.Domain.Models;
using MediatR;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Books
{
    public class GetBooksByAuthorHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AuthorBooksRequest, Pagination<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Pagination<BookDto>> Handle(AuthorBooksRequest request, CancellationToken cancellationToken)
        {
            var authorsBooks = await _unitOfWork.Books.GetByAuthorIdAsync(request.AuthorId);
            if (authorsBooks == null)
            {
                throw new EntityNotFoundException($"Books not found. AuthorId: {request.AuthorId}");
            }
            var books = _mapper.Map<List<BookDto>>(authorsBooks);
            return Pagination<BookDto>.ToPagedList(books, _mapper.Map<PageInfo>(request));
        }

    }
}
