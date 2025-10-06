using System.ComponentModel.DataAnnotations;

namespace YuGiOh.Infrastructure.Identity
{
    /// <summary>
    /// Configuration settings used for generating and validating JSON Web Tokens (JWT).
    /// </summary>
    public class JWTOptions
    {
        /// <summary>
        /// Secret key used to sign the JWT.
        /// Should be a long, randomly generated string (e.g., 32+ characters).
        /// </summary>
        [Required]
        [MinLength(32, ErrorMessage = "SecretKey should be at least 32 characters for security.")]
        public string SecretKey { get; set; }

        /// <summary>
        /// The issuer of the JWT (usually your app or domain).
        /// </summary>
        [Required]
        public string Issuer { get; set; }

        /// <summary>
        /// The intended audience for the JWT.
        /// </summary>
        [Required]
        public string Audience { get; set; }

        /// <summary>
        /// The duration in minutes before the access token expires.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "AccessTokenExpirationMinutes must be greater than 0.")]
        public int AccessTokenExpirationMinutes { get; set; }

        /// <summary>
        /// (Optional) The duration in days before the refresh token expires.
        /// Useful if you plan to implement refresh tokens.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "RefreshTokenExpirationDays must be greater than 0.")]
        public int RefreshTokenExpirationDays { get; set; }
    }
}
