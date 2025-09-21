/* src/Domain/Repositories/IUserRegistrationRepository.cs */

using System.Threading.Tasks;

using YuGiOh.Domain.Models.DTOs;

namespace YuGiOh.Domain.Repositories
{
    /// <summary>
    /// Defines the contract for user registration and email confirmation.
    /// The concrete implementation will use Identity Framework and email services.
    /// </summary>
    public interface IUserRegistrationRepository
    {
        /// <summary>
        /// Creates a new user account in the system based on the registration request.
        /// 
        /// The process should:
        /// 1. Validate the account data (email, password, etc.).
        /// 2. Create the Account entity in Identity Framework.
        /// 3. Assign appropriate roles depending on the account type (Player, Sponsor).
        /// 4. Create related domain entities:
        ///    - Player => domain Player entity with Address.
        ///    - Sponsor => domain Sponsor entity with IBAN.
        /// 5. Generate an email confirmation token.
        /// </summary>
        /// <param name="request">The registration request containing all necessary data.</param>
        /// <returns>
        /// The email confirmation token that must be sent to the user.
        /// </returns>
        Task<string> RegisterAsync(RegisterUserRequest request);

        /// <summary>
        /// Confirms the user's email based on the provided token.
        /// 
        /// The process should:
        /// 1. Validate that the email exists in the system.
        /// 2. Verify that the confirmation token is valid and not expired.
        /// 3. Mark the account as confirmed in Identity Framework.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="token">The confirmation token sent by email.</param>
        /// <returns>
        /// True if the confirmation succeeded, otherwise false.
        /// </returns>
        Task<bool> ConfirmEmailAsync(string email, string token);
    }
}
