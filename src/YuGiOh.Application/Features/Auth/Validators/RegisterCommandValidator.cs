using FluentValidation;

using YuGiOh.Domain.DTOs;
using YuGiOh.Application.Features.Auth.Commands;

namespace YuGiOh.Application.Features.Auth.Validators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Data).NotNull();

            RuleFor(x => x.Data.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Data.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.Data.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Data.FirstSurname)
                .NotEmpty().WithMessage("First surname is required.")
                .MaximumLength(100);

            RuleFor(x => x.Data.SecondSurname)
                .NotEmpty().WithMessage("Second surname is required.")
                .MaximumLength(100);

            RuleForEach(x => x.Data.Roles)
                .NotEmpty().WithMessage("Roles cannot contain empty values.");

            // Address (if provided)

            // IBAN (if sponsor role is present)
            When(x => x.Data.Roles.Contains("Sponsor"), () =>
            {
                RuleFor(x => x.Data.IBAN)
                    .NotEmpty().WithMessage("IBAN is required for sponsors.")
                    .Matches("^[A-Z0-9]+$").WithMessage("IBAN must be alphanumeric.");
            });
        }
    }
}
