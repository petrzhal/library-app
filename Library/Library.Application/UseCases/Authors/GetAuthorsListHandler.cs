using Library.Application.DTOs;
using MediatR;
using Library.Application.DTOs.Authors;
using Library.Domain.Interfaces.Repositories;
using AutoMapper;
using Library.Domain.Models;
using Library.Application.Common.Exceptions;

namespace Library.Application.UseCases.Authors
{
    public class GetAuthorsListHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AuthorListRequest, Pagination<AuthorDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Pagination<AuthorDto>> Handle(AuthorListRequest request, CancellationToken cancellationToken)
        {
            var pageInfo = _mapper.Map<PageInfo>(request);
            var authors = await _unitOfWork.Authors.GetByPageAsync(pageInfo);
            if (authors == null)
            {
                throw new EntityNotFoundException($"Authors not found. PageIndex: {request.PageIndex}, PageSize: {request.PageSize}");
            }

            var totalCount = await _unitOfWork.Authors.GetCountAsync();
            var authorDtos = _mapper.Map<List<AuthorDto>>(authors);
            return new Pagination<AuthorDto>(authorDtos, totalCount, pageInfo);
        }
    }
}
