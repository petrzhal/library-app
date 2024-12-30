using AutoMapper;
using Library.Application.Common.Interfaces.Services;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Application.DTOs.Book;
using MediatR;
using Library.Domain.Models;

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
            return Pagination<BookDto>.ToPagedList(_mapper.Map<List<BookDto>>(books), _mapper.Map<PageInfo>(request));
        }
    }
}
