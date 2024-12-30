using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs.Book;
using MediatR;

namespace Library.Application.UseCases.Books
{
    public class GetBookByIsbnHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<BookIsbnRequest, BookDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BookDto> Handle(BookIsbnRequest request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByISBNAsync(request.Isbn);
            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }
    }
}
