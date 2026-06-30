using FluentValidation;

namespace FCEService.Application.Features.Biometrics.Commands.IngestBiometrics;

public class IngestBiometricsCommandValidator : AbstractValidator<IngestBiometricsCommand>
{
    public IngestBiometricsCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("UserId is required.");

        RuleFor(x => x.BirthDate)
            .Must(x => DateTime.UtcNow.Year - x.Year >= 16).WithMessage("Age must be at least 16.");

        RuleFor(x => x.Weight)
            .InclusiveBetween(40, 200).WithMessage("Weight must be between 40 and 200 kg.");

        RuleFor(x => x.Height)
            .InclusiveBetween(140, 220).WithMessage("Height must be between 140 and 220 cm.");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid Gender.");

        RuleFor(x => x.Goal)
            .IsInEnum().WithMessage("Invalid Goal.");

        RuleFor(x => x.ActivityLevel)
            .IsInEnum().WithMessage("Invalid Activity Level.");
    }
}
