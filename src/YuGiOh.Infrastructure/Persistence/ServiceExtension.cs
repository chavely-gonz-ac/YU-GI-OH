using YuGiOh.Infrastructure.Persistence.Repositories;

using Ardalis.Specification;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YuGiOh.Infrastructure.Persistence
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuración de DbContext con PostgreSQL
            services.AddDbContext<YuGiOhDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(YuGiOhDbContext).Assembly.FullName)
                ));

            // Repositorios genéricos (DataRepository)
            services.AddScoped(typeof(IRepositoryBase<>), typeof(DataRepository<>));
            services.AddScoped(typeof(IReadRepositoryBase<>), typeof(DataRepository<>));

            // Seeder
            // services.AddHostedService<DataSeeder>();

            return services;
        }
    }
}
