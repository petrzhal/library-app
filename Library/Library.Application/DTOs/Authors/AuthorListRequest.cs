using MediatR; 

namespace Library.Application.DTOs.Authors
{
    public record AuthorListRequest(int PageIndex, int PageSize) : IRequest<Pagination<AuthorDto>>;
}
