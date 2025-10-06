using System.Threading.Tasks;
using YuGiOh.Domain.Models;

namespace YuGiOh.Domain.Services
{
    /// <summary>
    /// Defines contract for JWT and refresh token management.
    /// </summary>
    public interface IAccountTokensProvider
    {
        /// <summary>
        /// Generates a JWT access token for the given account.
        /// </summary>
        Task<string> GenerateJWTokenAsync(string accountId);

        /// <summary>
        /// Uses an old refresh token to issue a new one, verifying IP origin.
        /// </summary>
        Task<string> AddRefreshTokenAsync(string oldToken, string ipAddress);

        /// <summary>
        /// Issues a new refresh token directly for a specific account.
        /// </summary>
        Task<string> AddRefreshTokenByIdAsync(string accountId, string ipAddress);

        /// <summary>
        /// Revokes a refresh token (e.g., logout).
        /// </summary>
        Task RevokeRefreshTokenAsync(string token, string ipAddress);

        /// <summary>
        /// Retrieves refresh token data by token value.
        /// </summary>
        Task<RefreshTokenData?> GetRefreshTokenDataAsync(string token);
    }
}
