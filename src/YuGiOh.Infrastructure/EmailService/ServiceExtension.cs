using YuGiOh.Domain.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YuGiOh.Infrastructure.EmailService
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SMTPSettings>(configuration.GetSection("SMTPSettings"));
            services.AddScoped<IEmailSender, EmailSender>();

            return services;
        }
    }
}
