using FCEService.Application.Features.Biometrics.Commands.RegisterUserFitness;
using FCEService.Application.Features.Metrics.Queries.GetMetricsByUserId;
using FCEService.Application.Features.Plans.Queries.GetPlanConfigById;
using FCEService.Application.Features.Plans.Queries.GetPlanConfigs;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Entities.FitnessPlanConfig;

namespace FCEService.Application.Common.Mappings;

public static class MappingExtensions
{
    public static GetPlanConfigByIdResponse ToResponse(this FitnessPlanConfig plan)
    {
        return new GetPlanConfigByIdResponse(
            plan.PlanId,
            plan.PlanName,
            plan.Description,
            plan.Goal.ToString(),
            plan.Status.ToString(),
            plan.MinCalorie,
            plan.MaxCalorie,
            plan.EstimatedDuration,
            plan.WorkoutsPerWeek,
            plan.ProgramType
        );
    }

    public static PlanConfigDto ToDto(this FitnessPlanConfig plan)
    {
        return new PlanConfigDto(
            plan.PlanId,
            plan.PlanName,
            plan.Description,
            plan.Goal.ToString(),
            plan.Status.ToString(),
            plan.MinCalorie,
            plan.MaxCalorie,
            plan.EstimatedDuration,
            plan.WorkoutsPerWeek,
            plan.ProgramType
        );
    }

    public static GetMetricsByUserIdResponse ToResponse(this CalculatedMetrics metrics)
    {
        return new GetMetricsByUserIdResponse(
            metrics.UserId,
            metrics.Bmr,
            metrics.Tdee,
            metrics.CalorieTarget,
            metrics.Status.ToString(),
            metrics.CreatedAtUtc
        );
    }
}
