using MediatR;

using YuGiOh.Domain.Services;

namespace YuGiOh.Application.Features.Auth.Queries
{
    /// <summary>
    /// Query to confirm a user's email.
    /// </summary>
    public class ConfirmEmailQuery : IRequest<bool>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    /// <summary>
    /// Handler for confirming user emails.
    /// </summary>
    public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, bool>
    {
        private readonly IRegisterHandler _registerHandler;

        public ConfirmEmailQueryHandler(IRegisterHandler registerHandler)
        {
            _registerHandler = registerHandler 
                ?? throw new ArgumentNullException(nameof(registerHandler));
        }

        public async Task<bool> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            return await _registerHandler.ConfirmEmailAsync(request.Email, request.Token);
        }
    }
}
