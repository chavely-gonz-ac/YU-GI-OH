using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Ardalis.Specification;

using YuGiOh.Domain.Models;
using YuGiOh.Domain.Services;
using YuGiOh.Infrastructure.Identity;

namespace YuGiOh.Infrastructure.Identity.Services
{
    public class AccountTokensProvider : IAccountTokensProvider
    {
        private readonly IRepositoryBase<RefreshTokenData> _refreshTokenDataRepository;
        private readonly UserManager<Account> _userManager;
        private readonly JWTOptions _jwtOptions;

        public AccountTokensProvider(
            UserManager<Account> userManager,
            IOptions<JWTOptions> jwtOptions,
            IRepositoryBase<RefreshTokenData> refreshTokenDataRepository)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
            _refreshTokenDataRepository = refreshTokenDataRepository ?? throw new ArgumentNullException(nameof(refreshTokenDataRepository));
        }

        /// <inheritdoc/>
        public async Task<string> GenerateJWTokenAsync(string accountId)
        {
            if (accountId == null) throw new ArgumentNullException(nameof(accountId));
            var account = await _userManager.FindByIdAsync(accountId);
            if (account == null) throw new Exception($"There is none account related with Id:{accountId}");
            var relatedRoles = await _userManager.GetRolesAsync(account);

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Id),
                new Claim(JwtRegisteredClaimNames.Email, account.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add user roles as individual claims
            authClaims.AddRange(relatedRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
                claims: authClaims,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <inheritdoc/>
        public async Task<string> AddRefreshTokenAsync(string oldToken, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(oldToken))
                throw new ArgumentNullException(nameof(oldToken));

            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentException("IP address is required.", nameof(ipAddress));

            var refreshData = await _refreshTokenDataRepository.GetByIdAsync(oldToken);

            if (refreshData == null || !refreshData.IsActive)
                throw new Exception("Invalid or expired refresh token.");

            if (refreshData.CreatedByIp != ipAddress)
            {
                await _refreshTokenDataRepository.DeleteAsync(refreshData);
                throw new Exception("Invalid refresh token origin.");
            }
            var accountId = refreshData.AccountId;
            await _refreshTokenDataRepository.DeleteAsync(refreshData);

            // Issue a new refresh token
            var token = await AddRefreshTokenDataAsync(accountId, ipAddress);
            return token;
        }

        /// <inheritdoc/>
        public async Task<string> AddRefreshTokenByIdAsync(string accountId, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(accountId))
                throw new ArgumentNullException(nameof(accountId));

            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentException("IP address is required.", nameof(ipAddress));

            var spec = new RefreshTokenByAccountIdSpec(accountId);
            var existingToken = await _refreshTokenDataRepository.FirstOrDefaultAsync(spec);

            if (existingToken != null && existingToken.IsActive)
            {
                throw new Exception("An active refresh token already exists for this account.");
            }
            else if (existingToken != null)
            {
                await _refreshTokenDataRepository.DeleteAsync(existingToken);
            }

            return await AddRefreshTokenDataAsync(accountId, ipAddress);
        }

        /// <summary>
        /// Revokes a refresh token manually (e.g., logout).
        /// </summary>
        public async Task RevokeRefreshTokenAsync(string token, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));

            var refreshData = await _refreshTokenDataRepository.GetByIdAsync(token);
            if (refreshData == null || !refreshData.IsActive)
                throw new Exception("Invalid or expired refresh token.");

            refreshData.Revoked = DateTime.UtcNow;
            refreshData.RevokedByIp = ipAddress;

            await _refreshTokenDataRepository.UpdateAsync(refreshData);
        }

        /// <summary>
        /// Internal method to create and store refresh token securely.
        /// </summary>
        private async Task<string> AddRefreshTokenDataAsync(string accountId, string ipAddress)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            string token = Convert.ToBase64String(randomBytes);

            var tokenData = new RefreshTokenData()
            {
                Token = token,
                AccountId = accountId,
                Expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            await _refreshTokenDataRepository.AddAsync(tokenData);
            return token;
        }

        public async Task<RefreshTokenData?> GetRefreshTokenDataAsync(string token)
        {
            return await _refreshTokenDataRepository.GetByIdAsync(token);
        }

    }

    public sealed class RefreshTokenByAccountIdSpec : Specification<RefreshTokenData>
    {
        public RefreshTokenByAccountIdSpec(string accountId)
        {
            Query.Where(d => d.AccountId == accountId);
        }
    }
}