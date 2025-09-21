using YuGiOh.Domain.Models.DTOs;
using YuGiOh.Domain.Repositories;

using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace YuGiOh.Infrastructure.EmailService
{
    /// <summary>
    ///     Implements the <see cref="IEmailSender"/> interface using SMTP.
    ///     This repository is responsible for sending plain text emails.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly SMTPSettings _smtpSettings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        /// <param name="settings">The SMTP settings injected from configuration.</param>
        public EmailSender(IOptions<SMTPSettings> settings)
        {
            _smtpSettings = settings.Value;
        }

        /// <inheritdoc/>
        public async Task SendMailAsync(Email emailData)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_smtpSettings.FromDisplayName, _smtpSettings.Username));
            email.To.Add(MailboxAddress.Parse(emailData.ToAddress));
            email.Subject = emailData.Subject;


            if (emailData.IsHTML)
            {
                var builder = new BodyBuilder
                {
                    HtmlBody = emailData.Body,
                    TextBody = emailData.PlainTextBody ?? string.Empty
                };
                email.Body = builder.ToMessageBody();
            }
            else
            {
                email.Body = new TextPart("plain")
                {
                    Text = emailData.Body
                };
            }

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.FromAddress, _smtpSettings.Password); 
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
