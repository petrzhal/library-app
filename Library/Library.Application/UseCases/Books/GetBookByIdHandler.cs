using AutoMapper;
using Library.Application.DTOs.Book;
using MediatR;
using Library.Domain.Interfaces.Repositories;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Books
{
    public class GetBookByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<BookIdRequest, BookDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BookDto> Handle(BookIdRequest request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);
            if (book == null)
            {
                throw new EntityNotFoundException($"Book not found. BookId: {request.BookId}");
            }
            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }
    }
}
