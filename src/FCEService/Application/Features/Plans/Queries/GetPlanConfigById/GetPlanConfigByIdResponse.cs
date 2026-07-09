using System;

namespace FCEService.Application.Features.Plans.Queries.GetPlanConfigById;

public sealed record GetPlanConfigByIdResponse(
    Guid PlanId,
    string PlanName,
    string Description,
    string Goal,
    string Status,
    double MinCalorie,
    double MaxCalorie,
    string EstimatedDuration,
    int WorkoutsPerWeek,
    string ProgramType
);
