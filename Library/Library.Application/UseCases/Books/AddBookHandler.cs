using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.Common.Interfaces.Services;
using Library.Application.DTOs.Book;
using Library.Domain.Models;
using MediatR;

namespace Library.Application.UseCases.Books
{
    public class AddBookHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService) : IRequestHandler<AddBookRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Unit> Handle(AddBookRequest request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request);
            var image = _mapper.Map<Image>(request);

            var imageKey = $"BookImage:{Guid.NewGuid()}";

            image.Id = imageKey;

            await _cacheService.SetImageAsync(image, TimeSpan.FromDays(365));
            image.Id = imageKey;

            book.ImageId = imageKey;

            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
