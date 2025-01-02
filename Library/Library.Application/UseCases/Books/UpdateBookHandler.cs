using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.Common.Interfaces.Services;
using Library.Application.DTOs.Book;
using Library.Domain.Models;
using MediatR;

namespace Library.Application.UseCases.Books
{
    public class UpdateBookHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService) : IRequestHandler<UpdateBookRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Unit> Handle(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(request.Id);
            if (book == null)
                throw new KeyNotFoundException($"Book with Id {request.Id} not found.");

            var image = _mapper.Map<Image>(request);
            await _cacheService.SetImageAsync(image, TimeSpan.FromDays(365));

            _mapper.Map(request, book);

            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
