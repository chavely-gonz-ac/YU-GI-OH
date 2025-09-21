using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using YuGiOh.Infrastructure.Identity;
using YuGiOh.Infrastructure.Persistence;
using YuGiOh.Infrastructure.EmailService;

namespace YuGiOh.Infrastructure
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            services.AddIdentity();
            services.AddEmailService(configuration);

            return services;
        }
    }
}