using MediatR;

using YuGiOh.Domain.Models.DTOs;
using YuGiOh.Domain.ValueObjects;
using YuGiOh.Domain.Repositories;

namespace YuGiOh.Application.Features.Auth.Commands
{
    // The request (Command)
    public class SendConfirmationEmailCommand : IRequest
    {
        public string Email { get; set; }
        public string CallbackURL { get; set; }
    }

    // The handler
    public class SendConfirmationEmailCommandHandler : IRequestHandler<SendConfirmationEmailCommand>
    {
        private readonly IEmailSender _emailSender;

        public SendConfirmationEmailCommandHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            Email data = new ConfirmRegistrationEmail(request.Email, request.CallbackURL);
            await _emailSender.SendMailAsync(data);
        }
    }
}
