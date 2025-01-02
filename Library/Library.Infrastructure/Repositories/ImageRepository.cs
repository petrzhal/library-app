using Library.Application.Common.Interfaces.Repositories;
using Library.Domain.Models;
using Library.Infrastructure.Persistence;

namespace Library.Infrastructure.Repositories
{
    public class ImageRepository(LibraryDbContext context) : Repository<Image>(context), IImageRepository
    {
        public new async Task<string> AddAsync(Image image)
        {
            await _context.Set<Image>().AddAsync(image);
            await _context.SaveChangesAsync();
            return image.Id;
        }
    }
}
