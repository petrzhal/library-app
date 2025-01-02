using AutoMapper;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Interfaces.Services;
using Library.Application.DTOs;
using Library.Application.DTOs.Book;
using MediatR;
using Library.Domain.Models;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Books
{
    public class GetUsersBorrowedBooksHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService) : IRequestHandler<GetUsersBorrowedBooksRequest, Pagination<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<Pagination<BookDto>> Handle(GetUsersBorrowedBooksRequest request, CancellationToken cancellationToken)
        {
            var userId = _tokenService.GetUserIdFromAccessToken();

            var books = await _unitOfWork.Books.GetUsersBorrowedBooks(userId);
            if (books == null)
            {
                throw new EntityNotFoundException($"Books not found. PageIndex: {request.PageIndex}, PageSize: {request.PageSize}");
            }

            return Pagination<BookDto>.ToPagedList(_mapper.Map<List<BookDto>>(books), _mapper.Map<PageInfo>(request));
        }
    }
}
