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
        /// <summary>
        /// Query a logical "table" stored in Redis with caching + filtering.
        /// </summary>
        public async Task<ICollection<T>> GroupQuery<T>(
            Func<T, bool> predicate,
            Func<Task<ICollection<T>>> loader,
            TimeSpan? ttl = null,
            string? cacheKey = null)
        {
            if (cacheKey == null) cacheKey = GetCachePrefix(typeof(T).Name);
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (loader == null) throw new ArgumentNullException(nameof(loader));

            // Try cache first
            var cached = await GetAsync<ICollection<T>>(cacheKey).ConfigureAwait(false);

            ICollection<T> collection;
            if (cached != null && cached.Count > 0)
            {
                collection = cached;
            }
            else
            {
                collection = await loader().ConfigureAwait(false);

                if (collection != null && collection.Count > 0)
                {
                    var expiry = ttl ?? TimeSpan.FromSeconds(_options.DefaultTTLSeconds ?? 600);
                    await SetAsync(cacheKey, collection, expiry).ConfigureAwait(false);
                }
            }

            return collection.Where(predicate).ToList();
        }
    }
}