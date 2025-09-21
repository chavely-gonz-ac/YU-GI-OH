using MediatR;

using YuGiOh.Domain.Repositories;

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
        private readonly IUserRegistrationRepository _userRegistrationRepository;

        public ConfirmEmailQueryHandler(IUserRegistrationRepository userRegistrationRepository)
        {
            _userRegistrationRepository = userRegistrationRepository 
                ?? throw new ArgumentNullException(nameof(userRegistrationRepository));
        }

        public async Task<bool> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            return await _userRegistrationRepository.ConfirmEmailAsync(request.Email, request.Token);
        }
    }
}
