using MediatR;

using YuGiOh.Domain.DTOs;
using YuGiOh.Domain.Services;

namespace YuGiOh.Application.Features.Auth.Commands
{
    // The request (Command)
    public class RegisterCommand : IRequest<string>
    {
        public RegisterUserRequest Data { get; set; }
    }

    // The handler
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IRegisterHandler _registerHandler;

        public RegisterCommandHandler(IRegisterHandler registerHandler)
        {
            _registerHandler = registerHandler 
                ?? throw new ArgumentNullException(nameof(registerHandler));
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerUser = request.Data;

            // This will return the confirmation token
            return await _registerHandler.RegisterAsync(registerUser);
            
        }
    }
}
