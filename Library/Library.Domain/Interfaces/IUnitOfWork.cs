namespace Library.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IRoleRepository Roles { get; }
        IBookBorrowRepository BookBorrows { get; }
        Task<int> CompleteAsync();
    }
}
