using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.UserAssignedPlan;
using FCEService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FCEService.Application.Common.Interfaces;

public interface IPlanAssignmentService
{
    Task<Result<UserAssignedPlan>> AssignPlanAsync(
        Guid userId,
        Goal goal,
        FitnessStatus status,
        string reasonForChange,
        CancellationToken ct,
        string modifiedBy = "system");
}
