using YuGiOh.Domain.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YuGiOh.Infrastructure.CSCService
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddCSCService(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind CSCOptions from configuration
            services.Configure<CSCOptions>(configuration.GetSection("CSCOptions"));

            // Register CSCLoader
            services.AddSingleton<CSCLoader>();

            // Register CSCProvider as implementation of ICSCProvider
            services.AddScoped<ICSCProvider, CSCProvider>();

            return services;
        }
    }
}
