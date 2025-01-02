using Library.Application.Common.Interfaces.Repositories;
using Library.Application.Common.Interfaces;
using Library.Infrastructure.Repositories;

namespace Library.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public IUserRepository Users { get; private set; }
        public IBookRepository Books { get; private set; }
        public IAuthorRepository Authors { get; private set; }
        public IRefreshTokenRepository RefreshTokens { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IBookBorrowRepository BookBorrows { get; private set; }

        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Books = new BookRepository(_context);
            Authors = new AuthorRepository(_context);
            RefreshTokens = new RefreshTokenRepository(_context);
            Roles = new RoleRepository(_context);
            BookBorrows = new BookBorrowRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
