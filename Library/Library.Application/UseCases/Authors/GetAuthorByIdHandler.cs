using AutoMapper;
using MediatR;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs.Authors;

namespace Library.Application.UseCases.Books
{
    public class GetAuthorByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AuthorIdRequest, AuthorDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AuthorDto> Handle(AuthorIdRequest request, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId);
            return _mapper.Map<AuthorDto>(author);
        }
    }
}
