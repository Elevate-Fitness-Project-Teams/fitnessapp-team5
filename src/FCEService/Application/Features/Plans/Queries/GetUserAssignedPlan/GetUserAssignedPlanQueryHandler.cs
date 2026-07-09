using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.UserAssignedPlan;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FCEService.Application.Features.Plans.Queries.GetUserAssignedPlan;

public sealed class GetUserAssignedPlanQueryHandler(IAppDbContext dbContext)
    : IRequestHandler<GetUserAssignedPlanQuery, Result<GetUserAssignedPlanResponse>>
{
    public async Task<Result<GetUserAssignedPlanResponse>> Handle(
        GetUserAssignedPlanQuery query,
        CancellationToken ct)
    {
        var response = await dbContext.UserAssignedPlans
            .AsNoTracking()
            .Where(x => x.UserId == query.UserId && x.IsActive)
            .Join(
                dbContext.FitnessPlanConfigs.AsNoTracking(),
                userPlan => userPlan.PlanId,
                config => config.PlanId,
                (userPlan, config) => new GetUserAssignedPlanResponse(
                    userPlan.UserId,
                    userPlan.PlanId,
                    config.PlanName,
                    userPlan.IsActive
                )
            )
            .FirstOrDefaultAsync(ct);

        if (response is null)
            return UserAssignedPlanErrors.NotFound;

        return response;
    }
}
