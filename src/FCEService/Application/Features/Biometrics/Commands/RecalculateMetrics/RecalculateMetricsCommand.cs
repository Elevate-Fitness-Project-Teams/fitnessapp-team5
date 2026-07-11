using FCEService.Domain.Common.Results;
using MediatR;
using System;

namespace FCEService.Application.Features.Biometrics.Commands.RecalculateMetrics;

public sealed record RecalculateMetricsCommand(
    Guid UserId, 
    double NewWeight
) : IRequest<Result<Unit>>;
