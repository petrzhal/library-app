using AutoMapper;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Interfaces.Services;
using Library.Application.DTOs.Images;
using MediatR;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Images
{
    public class GetImageByBookIdHandler(IUnitOfWork unitOfWork, ICacheService cacheService, IMapper mapper) : IRequestHandler<GetImageRequest, ImageDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<ImageDto> Handle(GetImageRequest request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);
            if (book == null)
            {
                throw new EntityNotFoundException($"Book not found. BookId: {request.BookId}");
            }
            var cachedImage = await _cacheService.GetImageAsync(book.ImageId);
            if (cachedImage == null)
            {
                throw new EntityNotFoundException($"Image not found. ImageId: {book.ImageId}");
            }
            return _mapper.Map<ImageDto>(cachedImage);
        }
    }
}
