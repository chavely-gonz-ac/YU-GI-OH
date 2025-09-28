using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using YuGiOh.Infrastructure.Identity;
using YuGiOh.Infrastructure.Persistence;
using YuGiOh.Infrastructure.EmailService;
using YuGiOh.Infrastructure.CachingService;
using YuGiOh.Infrastructure.CSCService;

namespace YuGiOh.Infrastructure
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            services.AddIdentity();
            services.AddEmailService(configuration);
            services.AddCachingService(configuration);
            services.AddCSCService(configuration);

            return services;
        }
    }
}