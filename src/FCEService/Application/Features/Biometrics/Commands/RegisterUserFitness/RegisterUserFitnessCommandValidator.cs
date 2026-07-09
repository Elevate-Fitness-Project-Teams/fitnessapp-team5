using FluentValidation;

namespace FCEService.Application.Features.Biometrics.Commands.RegisterUserFitness;

using FCEService.Domain.Common.Constants;

public sealed class RegisterUserFitnessCommandValidator : AbstractValidator<RegisterUserFitnessCommand>
{
    public RegisterUserFitnessCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("UserId is required.");

        RuleFor(x => x.BirthDate)
            .Must(BeAtLeast16YearsOld).WithMessage($"Age must be at least {FCEConstants.Validation.MinAge}.");

        RuleFor(x => x.Weight)
            .InclusiveBetween(FCEConstants.Validation.MinWeight, FCEConstants.Validation.MaxWeight)
            .WithMessage($"Weight must be between {FCEConstants.Validation.MinWeight} and {FCEConstants.Validation.MaxWeight} kg.");

        RuleFor(x => x.Height)
            .InclusiveBetween(FCEConstants.Validation.MinHeight, FCEConstants.Validation.MaxHeight)
            .WithMessage($"Height must be between {FCEConstants.Validation.MinHeight} and {FCEConstants.Validation.MaxHeight} cm.");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid Gender.");

        RuleFor(x => x.Goal)
            .IsInEnum().WithMessage("Invalid Goal.");

        RuleFor(x => x.ActivityLevel)
            .IsInEnum().WithMessage("Invalid Activity Level.");
    }

    private bool BeAtLeast16YearsOld(DateTime birthDate)
    {
        var today = DateTime.UtcNow.Date;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }
        return age >= 16;
    }
}
