using FluentValidation;

namespace FCEService.Application.Features.Biometrics.Commands.RegisterUserFitness;

/// <summary>
/// Contract-level validation ONLY — checks that the request is well-formed before hitting the Domain.
/// Business rule validation (weight ranges, age limits, valid enum values) lives in UserFitnessStats.Create().
/// This prevents double-validation and keeps a single source of truth for domain rules.
/// </summary>
public sealed class RegisterUserFitnessCommandValidator : AbstractValidator<RegisterUserFitnessCommand>
{
    public RegisterUserFitnessCommandValidator()
    {
        // Contract: UserId must be a real Guid, not the default empty value
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("UserId is required.");

        // Contract: Weight, Height must be provided (non-zero) — range is validated by the Domain
        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight is required.");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height is required.");

        // Contract: BirthDate must be provided
        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .LessThan(DateTime.UtcNow).WithMessage("Birth date cannot be in the future.");

        // Contract: Enum values must be defined (prevents garbage data crashing the Domain)
        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid Gender value.");

        RuleFor(x => x.Goal)
            .IsInEnum().WithMessage("Invalid Goal value.");

        RuleFor(x => x.ActivityLevel)
            .IsInEnum().WithMessage("Invalid Activity Level value.");
    }
}
