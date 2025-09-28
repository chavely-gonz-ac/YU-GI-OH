using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

using YuGiOh.Domain.Repositories;

namespace YuGiOh.Infrastructure.CachingService
{
    public partial class RedisCachingService
    {
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var serialized = JsonSerializer.Serialize(value, _jsonOptions);
            var ttl = expiry ?? TimeSpan.FromSeconds(_options.DefaultTTLSeconds ?? 600);

            await _db.StringSetAsync(GetCacheKey(typeof(T).Name, key), serialized, ttl)
                     .ConfigureAwait(false);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var val = await _db.StringGetAsync(GetCacheKey(typeof(T).Name, key)).ConfigureAwait(false);
            if (!val.HasValue) return default;
            return JsonSerializer.Deserialize<T>(val, _jsonOptions);
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            // Fast path: try read
            var redisVal = await _db.StringGetAsync(GetCacheKey(typeof(T).Name, key)).ConfigureAwait(false);
            if (redisVal.HasValue)
            {
                return JsonSerializer.Deserialize<T>(redisVal!, _jsonOptions);
            }

            // Not present — create & set
            var result = await factory().ConfigureAwait(false);

            // Optionally don't cache nulls to avoid poisoning cache (change if you want to cache nulls)
            if (result != null)
            {
                var serialized = JsonSerializer.Serialize(result, _jsonOptions);
                var ttl = expiry ?? TimeSpan.FromSeconds(_options.DefaultTTLSeconds ?? 600);
                await _db.StringSetAsync(GetCacheKey(typeof(T).Name, key), serialized, ttl).ConfigureAwait(false);
            }

            return result;
        }

        public async Task RemoveAsync<T>(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            await _db.KeyDeleteAsync(GetCacheKey(typeof(T).Name, key)).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync<T>(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return await _db.KeyExistsAsync(GetCacheKey(typeof(T).Name, key)).ConfigureAwait(false);
        }
    }
}