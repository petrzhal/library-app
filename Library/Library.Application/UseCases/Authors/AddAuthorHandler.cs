using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs.Authors;
using Library.Domain.Models;
using MediatR;

namespace Library.Application.UseCases.Authors
{
    public class AddBookHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AddAuthorRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(AddAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = _mapper.Map<Author>(request);
            await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
