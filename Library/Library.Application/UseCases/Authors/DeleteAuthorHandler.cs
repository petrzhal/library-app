using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs.Authors;
using MediatR;

namespace Library.Application.UseCases.Books
{
    public class DeleteAuthorHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeleteAuthorRequest, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(DeleteAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId);
            await _unitOfWork.Authors.DeleteAsync(author);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
