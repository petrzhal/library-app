using AutoMapper;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Interfaces.Services;
using Library.Application.DTOs.Book;
using MediatR;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Books
{
    public class ReturnBookHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService) : IRequestHandler<ReturnBookRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<Unit> Handle(ReturnBookRequest request, CancellationToken cancellationToken)
        {
            var userId = _tokenService.GetUserIdFromAccessToken();

            var bookBorrow = await _unitOfWork.BookBorrows.GetByUserAndBookIdAsync(userId, request.BookId);
            if (bookBorrow == null)
            {
                throw new EntityNotFoundException($"BookBorrow not found. UserId: {userId}, BookId: {request.BookId}");
            }

            var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);
            if (book == null)
            {
                throw new EntityNotFoundException($"Book not found. BookId: {request.BookId}");
            }

            book.BorrowedAt = null;
            book.ReturnBy = null;

            await _unitOfWork.BookBorrows.DeleteAsync(bookBorrow);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
