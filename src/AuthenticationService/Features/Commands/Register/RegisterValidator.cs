using System.Data;
using FluentValidation;

namespace AuthenticationService.Features.Commands.Register
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("Last name is required.")
                    .MinimumLength(2).WithMessage("Last name must be at least 2 characters.")
                    .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                    .Matches("[0-9]").WithMessage("Password must contain at least one number.");

            RuleFor(x => x.PhoneNumber)
                    .NotEmpty().WithMessage("Phone number is required.")
                    .Matches(@"^(010|011|012|015)\d{8}$")
                    .WithMessage("Phone number must be a valid Egyptian mobile number.");
        }
    }
}
