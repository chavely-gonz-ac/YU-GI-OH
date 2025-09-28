using YuGiOh.Domain.Services;
using YuGiOh.Infrastructure.Persistence;
using YuGiOh.Infrastructure.Identity.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace YuGiOh.Infrastructure.Identity
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<Account, IdentityRole>(options =>
                {
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<YuGiOhDbContext>()
                .AddDefaultTokenProviders();


            services.AddScoped<IUserRegistrationHandler, UserRegistrationHandler>();

            return services;
        }
    }
}
