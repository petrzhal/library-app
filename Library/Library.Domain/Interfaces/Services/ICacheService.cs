using Library.Domain.Models;

namespace Library.Domain.Interfaces.Services
{
    public interface ICacheService
    {
        Task<Image> GetImageAsync(string key);
        Task SetImageAsync(Image image, TimeSpan expiration);
    }
}
