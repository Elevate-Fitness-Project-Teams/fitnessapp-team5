using FCEService.Domain.Common.Results;
using FCEService.Domain.Enums;
using MediatR;
using System;

namespace FCEService.Application.Features.Biometrics.Commands.UpdateUserFitnessStats;

public sealed record UpdateUserFitnessStatsCommand(
    Guid UserId,
    double Weight,
    double Height,
    DateTime BirthDate,
    Gender Gender,
    Goal Goal,
    ActivityLevel ActivityLevel,
    bool RequestNewPlan
) : IRequest<Result<Unit>>;
