using AutoMapper;
using Library.Domain.Interfaces.Repositories;
using Library.Application.DTOs.Book;
using MediatR;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Books
{
    public class GetBookByIsbnHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<BookIsbnRequest, BookDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BookDto> Handle(BookIsbnRequest request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByISBNAsync(request.Isbn);
            if (book == null)
            {
                throw new EntityNotFoundException($"Book not found. ISBN: {request.Isbn}");
            }
            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }
    }
}
