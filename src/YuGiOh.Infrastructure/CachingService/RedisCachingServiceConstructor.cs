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
    public partial class RedisCachingService : ICachingRepository
    {
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly IDatabase _db;
        private readonly RedisCacheOptions _options;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly string _instanceName;
        private readonly string _sep;

        public RedisCachingService(IConnectionMultiplexer multiplexer, IOptions<RedisCacheOptions> options)
        {
            _multiplexer = multiplexer ?? throw new ArgumentNullException(nameof(multiplexer));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _db = _multiplexer.GetDatabase();
            _instanceName = _options.InstanceName ?? string.Empty;
            _sep = string.IsNullOrEmpty(_instanceName) ? string.Empty : (_options.KeySeparator ?? ":");

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            };
        }

        public string GetCachePrefix(string tableName) => $"{_instanceName}:{tableName}";
        public string GetCacheKey(string tableName, string key) => $"{GetCachePrefix(tableName)}:{key}";
    }
}