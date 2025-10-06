using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using YuGiOh.Domain.Models;
using YuGiOh.Domain.Services;
using YuGiOh.Infrastructure.Identity.Services;

namespace YuGiOh.Infrastructure.Identity.Services
{
    /// <summary>
    /// Concrete implementation of authentication and authorization operations,
    /// built on ASP.NET Core Identity and JWT.
    /// </summary>
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly IAccountTokensProvider _accountTokensProvider;
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly JWTOptions _jwtOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationHandler"/> class.
        /// </summary>
        public AuthenticationHandler(
            IAccountTokensProvider accountTokensProvider,
            UserManager<Account> userManager,
            SignInManager<Account> signInManager,
            IOptions<JWTOptions> jwtOptions)
        {
            _accountTokensProvider = accountTokensProvider ?? throw new ArgumentNullException(nameof(accountTokensProvider));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        /// <inheritdoc/>
        public async Task<(string AccessToken, string RefreshToken)> AuthenticateAsync(string handler, string password, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(handler))
                throw new Exception("Username or email is required.");
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required.");

            var account = await GetAccount(handler);

            var result = await _signInManager.CheckPasswordSignInAsync(account, password, lockoutOnFailure: true);
            if (!result.Succeeded)
                throw new Exception("Invalid credentials.");

            // Generate JWT access token
            var accessToken = await _accountTokensProvider.GenerateJWTokenAsync(account.Id);

            // Generate refresh token
            var refreshToken = await _accountTokensProvider.AddRefreshTokenByIdAsync(account.Id, ipAddress);

            return (accessToken, refreshToken);
        }

        private async Task<Account> GetAccount(string handler)
        {
            // Support both email or username
            var account = await _userManager.FindByNameAsync(handler)
                           ?? await _userManager.FindByEmailAsync(handler);

            if (account == null)
                throw new Exception("Invalid credentials. User not found.");

            if (!await _userManager.IsEmailConfirmedAsync(account))
                throw new Exception("Email is not confirmed.");

            if (account.Statement == Domain.Enums.AccountStatement.Deleted)
                throw new Exception("Account has been deleted.");

            if (account.Statement == Domain.Enums.AccountStatement.Inactive)
                throw new Exception("Account is inactive.");

            return account;
        }



        /// <summary>
        /// Refresh access token using a valid refresh token.
        /// </summary>
        public async Task<(string AccessToken, string RefreshToken)> RefreshAsync(string refreshToken, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new Exception("Refresh token is required.");
            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new Exception("IP address is required.");

            // Replace old refresh token with new one
            var newRefreshToken = await _accountTokensProvider.AddRefreshTokenAsync(refreshToken, ipAddress);

            // Retrieve refresh token data to identify account
            var refreshData = await _accountTokensProvider.GetRefreshTokenDataAsync(newRefreshToken);
            if (refreshData == null || !refreshData.IsActive)
                throw new Exception("Invalid or expired refresh token.");
            var accessToken = await _accountTokensProvider.GenerateJWTokenAsync(refreshData.AccountId);

            return (accessToken, newRefreshToken);
        }

    }
}
