using AutoMapper;
using Library.Application.Common.Interfaces.Services;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs.Book;
using MediatR;

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

            var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);

            book.BorrowedAt = null;
            book.ReturnBy = null;

            await _unitOfWork.BookBorrows.DeleteAsync(bookBorrow);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
