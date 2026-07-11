using FCEService.Domain.Enums;

namespace FCEService.Application.Features.Metrics.Queries.GetMetricsByUserId;

public sealed record GetMetricsByUserIdResponse(
    Guid UserId,
    double Bmr,
    double Tdee,
    double CalorieTarget,
    string Status,
    DateTimeOffset CalculatedAt
);
