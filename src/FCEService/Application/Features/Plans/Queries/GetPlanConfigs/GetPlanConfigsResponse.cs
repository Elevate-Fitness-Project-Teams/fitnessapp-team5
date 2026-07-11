using System;
using System.Collections.Generic;

namespace FCEService.Application.Features.Plans.Queries.GetPlanConfigs;

public sealed record PlanConfigDto(
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

public sealed record GetPlanConfigsResponse(
    List<PlanConfigDto> Plans,
    int TotalCount,
    int PageNumber,
    int PageSize
);
