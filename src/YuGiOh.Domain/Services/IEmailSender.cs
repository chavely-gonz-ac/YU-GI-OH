using YuGiOh.Domain.DTOs;

namespace YuGiOh.Domain.Services
{
    /// <summary>
    ///     Defines the contract for sending emails.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        ///     Sends an email using the provided email data.
        /// </summary>
        /// <param name="emailData">
        ///     The email data containing recipient address, subject, and body.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous send operation.
        /// </returns>
        Task SendMailAsync(Email emailData);
    }
}
