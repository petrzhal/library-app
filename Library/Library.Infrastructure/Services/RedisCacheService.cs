using Library.Application.Common.Interfaces.Services;
using Library.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Library.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisCacheService(IConfiguration configuration)
        {
            var connectionString = configuration["Redis:ConnectionString"];
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _database = _redis.GetDatabase();
        }

        public async Task<Image> GetImageAsync(string key)
        {
            var json = await _database.StringGetAsync(key);
            if (json.IsNull)
            {
                return null;
            }
            var image = json.IsNull ? null : JsonConvert.DeserializeObject<CachedImage>(json);
            return new Image
            {
                Id = key,
                ImageData = image.ImageData,
                ImageType = image.ImageType
            };
        }

        public async Task SetImageAsync(Image image, TimeSpan expiration)
        {
            var cachedImage = new CachedImage
            {
                ImageData = image.ImageData,
                ImageType = image.ImageType,
            };

            var json = JsonConvert.SerializeObject(cachedImage);
            await _database.StringSetAsync(image.Id, json, expiration);
        }
    }
}