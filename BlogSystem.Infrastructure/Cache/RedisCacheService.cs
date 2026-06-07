using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Interfaces;
using StackExchange.Redis;
using System.Text.Json;
namespace BlogSystem.Infrastructure.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _redis;

        public RedisCacheService(IConnectionMultiplexer connection)
        {
            _redis = connection.GetDatabase();
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _redis.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;
            return JsonSerializer.Deserialize<T>(value!);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            await _redis.StringSetAsync(key, JsonSerializer.Serialize(value), expiration);
        }

        public async Task RemoveAsync(string key)
        {
            await _redis.KeyDeleteAsync(key);
        }
    }
}
