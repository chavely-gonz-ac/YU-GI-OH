using MediatR;
using YuGiOh.Domain.Services;

namespace YuGiOh.Application.Features.Auth.Commands
{
    // Request (Command)
    public class AuthenticateCommand : IRequest<(string AccessToken, string RefreshToken)>
    {
        public string Handler { get; set; }  // email or username
        public string Password { get; set; }
        public string? IpAddress { get; set; }
    }

    // Handler
    public class AuthenticateCommandHandler 
        : IRequestHandler<AuthenticateCommand, (string AccessToken, string RefreshToken)>
    {
        private readonly IAuthenticationHandler _authenticationHandler;

        public AuthenticateCommandHandler(IAuthenticationHandler authenticationHandler)
        {
            _authenticationHandler = authenticationHandler 
                ?? throw new ArgumentNullException(nameof(authenticationHandler));
        }

        public async Task<(string AccessToken, string RefreshToken)> Handle(
            AuthenticateCommand request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return await _authenticationHandler.AuthenticateAsync(
                request.Handler,
                request.Password,
                request.IpAddress
            );
        }
    }
}
