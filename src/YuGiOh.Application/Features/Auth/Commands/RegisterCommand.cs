using MediatR;

using YuGiOh.Domain.Models.DTOs;
using YuGiOh.Domain.Repositories;

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
        private readonly IUserRegistrationRepository _userRegistrationRepository;

        public RegisterCommandHandler(IUserRegistrationRepository userRegistrationRepository)
        {
            _userRegistrationRepository = userRegistrationRepository;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("I am in command.");
            var registerUser = request.Data;

            // This will return the confirmation token
            return await _userRegistrationRepository.RegisterAsync(registerUser);
        }
    }
}
