using FluentValidation;

namespace FCEService.Application.Features.Biometrics.Commands.UpdateUserFitnessStats;

/// <summary>
/// Contract-level validation ONLY.
/// Business rules (weight/height ranges) are enforced by the Domain entity's Update() method.
/// </summary>
public sealed class UpdateUserFitnessStatsCommandValidator : AbstractValidator<UpdateUserFitnessStatsCommand>
{
    public UpdateUserFitnessStatsCommandValidator()
    {
        // Contract: UserId must not be the default empty Guid
        // NotEqual(Guid.Empty) is more explicit and reliable than NotEmpty() for Guid types
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("User ID is required.");

        // Contract: non-zero required — exact ranges enforced by Domain
        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight is required.");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height is required.");

        // Contract: BirthDate must be in the past
        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .LessThan(DateTime.UtcNow).WithMessage("Birth date cannot be in the future.");

        // Contract: Enum values must be defined
        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Gender must be a valid option.");

        RuleFor(x => x.Goal)
            .IsInEnum().WithMessage("Goal must be a valid option.");

        RuleFor(x => x.ActivityLevel)
            .IsInEnum().WithMessage("Activity level must be a valid option.");
    }
}
