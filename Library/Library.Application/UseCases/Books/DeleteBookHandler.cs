using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs.Book;
using MediatR;

namespace Library.Application.UseCases.Books
{
    public class DeleteBookHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeleteBookRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(DeleteBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);
            await _unitOfWork.Books.DeleteAsync(book);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
