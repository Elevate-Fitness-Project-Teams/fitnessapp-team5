using System;
using FCEService.Domain.Enums;

namespace FCEService.Application.Features.Biometrics.Commands.RegisterUserFitness;

public sealed record RegisterUserFitnessResponse(
    Guid UserId,
    double Bmr,
    double Tdee,
    double CalorieTarget,
    FitnessStatus Status,
    Guid? AssignedPlanId,
    string? AssignedPlanName
);
