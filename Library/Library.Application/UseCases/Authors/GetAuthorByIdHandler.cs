using AutoMapper;
using MediatR;
using Library.Domain.Interfaces.Repositories;
using Library.Application.DTOs.Authors;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Authors
{
    public class GetAuthorByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AuthorIdRequest, AuthorDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AuthorDto> Handle(AuthorIdRequest request, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId);
            if (author == null)
            {
                throw new EntityNotFoundException($"Author not found. AuthorId: {request.AuthorId}");
            }
            return _mapper.Map<AuthorDto>(author);
        }
    }
}
