using AutoMapper;
using Library.Application.DTOs.Book;
using MediatR;
using Library.Application.Common.Interfaces;

namespace Library.Application.UseCases.Books
{
    public class GetBookByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<BookIdRequest, BookDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BookDto> Handle(BookIdRequest request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);
            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }
    }
}
