using FluentValidation;
using System;

namespace FCEService.Application.Features.Biometrics.Queries.GetBiometrics;

public class GetBiometricsByIdValidator : AbstractValidator<GetBiometricsByIdQuery>
{
    public GetBiometricsByIdValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("UserId is required.");
    }
}
