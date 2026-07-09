using FluentValidation;

namespace FCEService.Application.Features.Biometrics.Commands.UpdateUserFitnessStats;

public sealed class UpdateUserFitnessStatsCommandValidator : AbstractValidator<UpdateUserFitnessStatsCommand>
{
    public UpdateUserFitnessStatsCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Weight)
            .InclusiveBetween(40, 200).WithMessage("Weight must be between 40 and 200 kg.");

        RuleFor(x => x.Height)
            .InclusiveBetween(140, 220).WithMessage("Height must be between 140 and 220 cm.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Gender must be a valid option.");

        RuleFor(x => x.Goal)
            .IsInEnum().WithMessage("Goal must be a valid option.");

        RuleFor(x => x.ActivityLevel)
            .IsInEnum().WithMessage("Activity level must be a valid option.");
    }
}
