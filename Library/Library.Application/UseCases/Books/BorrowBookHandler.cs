using AutoMapper;
using Library.Application.Common.Exceptions;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Interfaces.Services;
using Library.Application.DTOs.Book;
using Library.Domain.Models;
using MediatR;

namespace Library.Application.UseCases.Books
{
    public class BorrowBookHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService) : IRequestHandler<BorrowBookRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<Unit> Handle(BorrowBookRequest request, CancellationToken cancellationToken)
        {
            var userId = _tokenService.GetUserIdFromAccessToken();
            var bookBorrow = new BookBorrow { BookId = request.BookId, UserId = userId };

            var oldBookBorrow = await _unitOfWork.BookBorrows.GetByUserAndBookIdAsync(userId, bookBorrow.BookId);
            if (oldBookBorrow != null)
            {
                throw new BookAlreadyBorrowedException(request.BookId.ToString());
            }
            var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);
            if (book == null)
            {
                throw new EntityNotFoundException($"Book not found. BookId: {request.BookId}");
            }

            await _unitOfWork.BookBorrows.AddAsync(bookBorrow);

            book.BorrowedAt = DateTime.UtcNow;
            book.ReturnBy = DateTime.UtcNow.AddDays(30);

            await _unitOfWork.Books.UpdateAsync(book);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
