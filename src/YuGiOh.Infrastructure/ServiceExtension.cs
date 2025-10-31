using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YuGiOh.Infrastructure.Identity;
using YuGiOh.Infrastructure.Persistence;
using YuGiOh.Infrastructure.Seeding;

namespace YuGiOh.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering the Infrastructure layer services,
    /// including database persistence and identity management.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Registers the Infrastructure services into the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to which dependencies will be added.</param>
        /// <param name="configuration">The application configuration instance.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Persistence Layer (EF Core, DbContext, Repositories)
            services.AddPersistence(configuration);

            // Register Identity Layer (UserManager, RoleManager, etc.)
            services.AddIdentity(configuration);

            services.AddSeeding();

            return services;
        }
    }
}
