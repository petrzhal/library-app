using AutoMapper;
using Library.Domain.Interfaces.Repositories;
using Library.Application.DTOs.Authors;
using MediatR;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Authors
{
    public class DeleteAuthorHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeleteAuthorRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(DeleteAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId);
            if (author == null)
            {
                throw new EntityNotFoundException($"Author not found. AuthorId: {request.AuthorId}");
            }
            await _unitOfWork.Authors.DeleteAsync(author);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
