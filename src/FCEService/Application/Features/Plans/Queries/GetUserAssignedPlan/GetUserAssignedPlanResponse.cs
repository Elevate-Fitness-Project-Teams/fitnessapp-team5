using System;

namespace FCEService.Application.Features.Plans.Queries.GetUserAssignedPlan;

public sealed record GetUserAssignedPlanResponse(
    Guid UserId,
    Guid PlanId,
    string PlanName,
    bool IsActive
);
