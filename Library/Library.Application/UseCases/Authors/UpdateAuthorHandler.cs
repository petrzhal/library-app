using AutoMapper;
using Library.Domain.Interfaces.Repositories;
using Library.Application.DTOs.Authors;
using Library.Domain.Models;
using MediatR;

namespace Library.Application.UseCases.Authors
{
    public class UpdateAuthorHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateAuthorRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpdateAuthorRequest request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Authors.UpdateAsync(_mapper.Map<Author>(request));
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
