using YuGiOh.Domain.DataToObject;

namespace YuGiOh.Domain.Services
{
    /// <summary>
    /// Defines the contract for handling user registration and email confirmation operations.
    /// </summary>
    public interface IRegisterHandler
    {
        /// <summary>
        /// Registers a new user account with the system.
        /// </summary>
        /// <param name="request">
        /// The <see cref="RegisterRequest"/> containing user credentials and role assignments.
        /// </param>
        /// <returns>A task that represents the asynchronous registration operation.</returns>
        /// <exception cref="Exceptions.APIException">
        /// Thrown when validation fails, a duplicate account exists, or any required data is missing.
        /// </exception>
        Task RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Confirms a user’s email address using a token.
        /// </summary>
        /// <param name="email">The email of the user to confirm.</param>
        /// <param name="token">The confirmation token issued by Identity.</param>
        /// <returns>
        /// <see langword="true"/> if the confirmation succeeds;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="Exceptions.APIException">
        /// Thrown when either the email or token is null or whitespace.
        /// </exception>
        Task<bool> ConfirmEmailAsync(string email, string token);
    }
}
