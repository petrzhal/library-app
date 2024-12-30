using Library.Application.Common.Interfaces.Services;
using Library.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace Library.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisCacheService(IConfiguration configuration)
        {
            var connectionString = configuration["Redis:ConnectionString"];
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Redis connection string is not configured.", nameof(configuration));

            _redis = ConnectionMultiplexer.Connect(connectionString);
            _database = _redis.GetDatabase();
        }

        public async Task<Image> GetImageAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            try
            {
                var json = await _database.StringGetAsync(key);
                if (json.IsNullOrEmpty)
                    return null;

                var cachedImage = JsonConvert.DeserializeObject<CachedImage>(json);
                if (cachedImage == null)
                    return null;

                return new Image
                {
                    Id = key,
                    ImageData = cachedImage.ImageData,
                    ImageType = cachedImage.ImageType
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving image with key '{key}': {ex.Message}");
                return null;
            }
        }

        public async Task SetImageAsync(Image image, TimeSpan expiration)
        {
            if (image == null || string.IsNullOrWhiteSpace(image.Id))
                return;

            try
            {
                var cachedImage = new CachedImage
                {
                    ImageData = image.ImageData,
                    ImageType = image.ImageType,
                };

                var json = JsonConvert.SerializeObject(cachedImage);
                await _database.StringSetAsync(image.Id, json, expiration);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting image with key '{image.Id}': {ex.Message}");
            }
        }
    }
}
