using MediatR;
using YuGiOh.Domain.Services;

namespace YuGiOh.Application.Features.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<(string AccessToken, string RefreshToken)>
    {
        public string RefreshToken { get; set; }
        public string IpAddress { get; set; }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, (string AccessToken, string RefreshToken)>
    {
        private readonly IAuthenticationHandler _authenticationHandler;

        public RefreshTokenCommandHandler(IAuthenticationHandler authenticationHandler)
        {
            _authenticationHandler = authenticationHandler ?? throw new ArgumentNullException(nameof(authenticationHandler));
        }

        public async Task<(string AccessToken, string RefreshToken)> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return await _authenticationHandler.RefreshAsync(request.RefreshToken, request.IpAddress);
        }
    }
}
