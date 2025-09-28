using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YuGiOh.Domain.Repositories
{
    /// <summary>
    /// Defines the contract for a caching repository abstraction.
    /// This interface is implemented by <see cref="YuGiOh.Infrastructure.CachingService.RedisCachingService"/>.
    /// It exposes generic methods for storing, retrieving, removing, and querying cached data.
    /// </summary>
    public interface ICachingRepository
    {
        /// <summary>
        /// Stores a value in the cache for the given key.
        /// </summary>
        /// <typeparam name="T">Type of the value to store.</typeparam>
        /// <param name="key">Cache key (unique identifier).</param>
        /// <param name="value">Value to cache.</param>
        /// <param name="expiry">Optional expiration time. Defaults to configured TTL.</param>
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// Retrieves a cached value for the given key.
        /// </summary>
        /// <typeparam name="T">Expected type of the value.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <returns>The cached value if found; otherwise <c>default</c>.</returns>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Retrieves a cached value for the given key, or creates and caches it if not found.
        /// </summary>
        /// <typeparam name="T">Expected type of the value.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <param name="factory">Async factory that produces the value if it is not cached.</param>
        /// <param name="expiry">Optional expiration time. Defaults to configured TTL.</param>
        /// <returns>The cached or newly created value.</returns>
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null);

        /// <summary>
        /// Removes a cached value for the given key.
        /// </summary>
        /// <typeparam name="T">Type of the cached value (used to compose the cache key).</typeparam>
        /// <param name="key">Cache key.</param>
        Task RemoveAsync<T>(string key);

        /// <summary>
        /// Checks whether a cached value exists for the given key.
        /// </summary>
        /// <typeparam name="T">Type of the cached value (used to compose the cache key).</typeparam>
        /// <param name="key">Cache key.</param>
        /// <returns><c>true</c> if the cache entry exists; otherwise <c>false</c>.</returns>
        Task<bool> ExistsAsync<T>(string key);

        /// <summary>
        /// Queries a logical "table" stored in Redis with caching + filtering.
        /// Useful for caching entire collections and filtering them at query time.
        /// </summary>
        /// <typeparam name="T">Type of the objects being cached.</typeparam>
        /// <param name="predicate">Filter applied to cached data.</param>
        /// <param name="loader">Async function to load data when cache is empty.</param>
        /// <param name="ttl">Optional time-to-live for caching the collection. Defaults to configured TTL.</param>
        /// <returns>Filtered collection of <typeparamref name="T"/>.</returns>
        Task<ICollection<T>> GroupQuery<T>(
            Func<T, bool> predicate,
            Func<Task<ICollection<T>>> loader,
            TimeSpan? ttl = null,
            string? cacheKey = null);
    }
}