using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.Common.Interfaces.Services;
using Library.Application.DTOs.Images;
using MediatR;

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
            var cachedImage = await _cacheService.GetImageAsync(book.ImageId);
            return _mapper.Map<ImageDto>(cachedImage);
        }
    }
}
