using YuGiOh.Domain.DTOs;
using YuGiOh.Domain.Services;

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
        private readonly SMTPOptions _smtpOptions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        /// <param name="settings">The SMTP settings injected from configuration.</param>
        public EmailSender(IOptions<SMTPOptions> options)
        {
            _smtpOptions = options.Value;
        }

        /// <inheritdoc/>
        public async Task SendMailAsync(Email emailData)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_smtpOptions.FromDisplayName, _smtpOptions.Username));
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
            await smtp.ConnectAsync(_smtpOptions.Server, _smtpOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpOptions.FromAddress, _smtpOptions.Password); 
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
