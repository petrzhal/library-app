using Moq;
using AutoMapper;
using Library.Domain.Interfaces.Repositories;

namespace Library.Application.Tests.UseCases
{
    public abstract class HandlerTestBase
    {
        protected readonly Mock<IUnitOfWork> UnitOfWorkMock = new Mock<IUnitOfWork>();
        protected readonly Mock<IMapper> MapperMock = new Mock<IMapper>();
    }
}
