namespace YuGiOh.Infrastructure.CachingService
{
    public class RedisCacheOptions
    {
        /// <summary>
        /// The redis connection string (e.g. "localhost:6379,abortConnect=false") or ConfigurationOptions string.
        /// </summary>
        public string Configuration { get; set; } = "localhost:6379";

        /// <summary>
        /// Prefix for keys (helps multi-tenant or multi-app)
        /// </summary>
        public string? InstanceName { get; set; }

        /// <summary>
        /// Default TTL in seconds when none specified
        /// </summary>
        public int? DefaultTTLSeconds { get; set; } = 600;

        /// <summary>
        /// Separator between instance name and key
        /// </summary>
        public string KeySeparator { get; set; } = ":";
    }
}