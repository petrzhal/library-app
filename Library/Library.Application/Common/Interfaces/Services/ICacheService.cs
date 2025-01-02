using Library.Domain.Models;

namespace Library.Application.Common.Interfaces.Services
{
    public interface ICacheService
    {
        Task<Image> GetImageAsync(string key);
        Task SetImageAsync(Image image, TimeSpan expiration);
    }
}
