// using System;
// using System.Text;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Http; // ✅ Needed for StatusCodes + WriteAsync
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration; // ✅ Needed for IConfiguration
using Microsoft.Extensions.DependencyInjection;
// using Microsoft.IdentityModel.Tokens;
// using Newtonsoft.Json;

using YuGiOh.Domain.Services;
using YuGiOh.Infrastructure.Identity.Services;
using YuGiOh.Infrastructure.Persistence;
namespace YuGiOh.Infrastructure.Identity
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            // Identity Core Setup
            services
                .AddIdentity<Account, IdentityRole>(options =>
                {
                    // User settings
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;

                    // Sign-in settings
                    options.SignIn.RequireConfirmedEmail = true;

                    // Password policy (stronger than default)
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 8;

                    // Lockout policy
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    options.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<YuGiOhDbContext>()
                .AddDefaultTokenProviders();

            #region JWT Service
            // services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));
            // var jwtOptions = configuration.GetSection("JWTOptions").Get<JWTOptions>();

            // if (string.IsNullOrWhiteSpace(jwtOptions.SecretKey) || jwtOptions.SecretKey.Length < 32)
            // {
            //     throw new InvalidOperationException("JWT SecretKey must be at least 32 characters.");
            // }

            // services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            // })
            // .AddJwtBearer(o =>
            // {
            //     o.RequireHttpsMetadata = true; // ✅ enforce HTTPS in production
            //     o.SaveToken = true;

            //     o.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuerSigningKey = true,
            //         ValidateIssuer = true,
            //         ValidateAudience = true,
            //         ValidateLifetime = true,
            //         ClockSkew = TimeSpan.FromMinutes(1), // small tolerance
            //         ValidIssuer = jwtOptions.Issuer,
            //         ValidAudience = jwtOptions.Audience,
            //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
            //     };

            //     o.Events = new JwtBearerEvents()
            //     {
            //         OnAuthenticationFailed = context =>
            //         {
            //             context.NoResult();
            //             context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //             context.Response.ContentType = "application/json";
            //             var result = JsonConvert.SerializeObject(new { error = "Authentication failed", details = context.Exception.Message });
            //             return context.Response.WriteAsync(result);
            //         },
            //         OnChallenge = context =>
            //         {
            //             context.HandleResponse();
            //             context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //             context.Response.ContentType = "application/json";
            //             var result = JsonConvert.SerializeObject(new { error = "Unauthorized", message = "You must be logged in to access this resource." });
            //             return context.Response.WriteAsync(result);
            //         },
            //         OnForbidden = context =>
            //         {
            //             context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //             context.Response.ContentType = "application/json";
            //             var result = JsonConvert.SerializeObject(new { error = "Forbidden", message = "You do not have access to this resource." });
            //             return context.Response.WriteAsync(result);
            //         }
            //     };
            // });
            #endregion

            services.AddScoped<IRegisterHandler, RegisterHandler>();
            // services.AddScoped<IAccountTokensProvider, AccountTokensProvider>();
            // services.AddScoped<IAuthenticationHandler, AuthenticationHandler>();

            return services;
        }
    }
}
