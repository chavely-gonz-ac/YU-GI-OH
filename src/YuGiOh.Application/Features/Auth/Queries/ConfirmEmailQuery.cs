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
        private readonly IUserRegistrationHandler _userRegistrationHandler;

        public ConfirmEmailQueryHandler(IUserRegistrationHandler userRegistrationHandler)
        {
            _userRegistrationHandler = userRegistrationHandler 
                ?? throw new ArgumentNullException(nameof(userRegistrationHandler));
        }

        public async Task<bool> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            return await _userRegistrationHandler.ConfirmEmailAsync(request.Email, request.Token);
        }
    }
}
