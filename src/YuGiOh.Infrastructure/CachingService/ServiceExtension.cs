using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using StackExchange.Redis;

using YuGiOh.Domain.Repositories;

namespace YuGiOh.Infrastructure.CachingService
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind options
            services.Configure<RedisCacheOptions>(configuration.GetSection("RedisCacheOptions"));
            var opts = configuration.GetSection("RedisCacheOptions").Get<RedisCacheOptions>() ?? new RedisCacheOptions();

            // Build connection options carefully
            var cfg = ConfigurationOptions.Parse(opts.Configuration);
            cfg.AbortOnConnectFail = false; // safer for many environments

            // Connect — we create a singleton multiplexer
            var multiplexer = ConnectionMultiplexer.Connect(cfg);

            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddSingleton<ICachingRepository, RedisCachingService>();

            return services;
        }
    }
}
