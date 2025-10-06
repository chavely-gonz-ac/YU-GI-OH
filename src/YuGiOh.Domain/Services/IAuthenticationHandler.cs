using System.Threading.Tasks;

namespace YuGiOh.Domain.Services
{
    /// <summary>
    /// Defines contract for authentication operations using Identity + JWT.
    /// </summary>
    public interface IAuthenticationHandler
    {
        /// <summary>
        /// Authenticate a user using username/email and password, returning access & refresh tokens.
        /// </summary>
        /// <param name="handler">Email or username.</param>
        /// <param name="password">User password.</param>
        /// <param name="ipAddress">Request IP address.</param>
        Task<(string AccessToken, string RefreshToken)> AuthenticateAsync(string handler, string password, string ipAddress);

        /// <summary>
        /// Refresh an access token using a valid refresh token.
        /// </summary>
        /// <param name="refreshToken">Existing refresh token.</param>
        /// <param name="ipAddress">Request IP address.</param>
        Task<(string AccessToken, string RefreshToken)> RefreshAsync(string refreshToken, string ipAddress);
    }
}
