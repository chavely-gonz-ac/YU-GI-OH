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
        private readonly IUserRegistrationHandler _userRegistrationHandler;

        public RegisterCommandHandler(IUserRegistrationHandler userRegistrationHandler)
        {
            _userRegistrationHandler = userRegistrationHandler;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerUser = request.Data;

            // This will return the confirmation token
            return await _userRegistrationHandler.RegisterAsync(registerUser);
            
        }
    }
}
