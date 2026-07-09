using System;

namespace FCEService.Application.IntegrationEvents;

public sealed record WeightUpdatedIntegrationEvent(
    Guid UserId,
    double NewWeight,
    DateTimeOffset LoggedAt
);
