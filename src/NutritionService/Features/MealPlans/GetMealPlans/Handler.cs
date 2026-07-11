using Microsoft.EntityFrameworkCore;
using NutritionService.Common.DTOs;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Persistence;

namespace NutritionService.Features.MealPlans.GetMealPlans
{
    public static class GetMealPlansHandler
    {
        public static async Task<IResult> HandleAsync(
            NutritionDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var plans = await dbContext.MealPlans
                .AsNoTracking()
                .Include(plan => plan.MealPlanItems)
                .ThenInclude(item => item.Meal)
                .OrderBy(plan => plan.TargetCalorieRangeMin)
                .ThenBy(plan => plan.Name)
                .ToListAsync(cancellationToken);

            return Results.Ok(ApiResponse<IReadOnlyList<MealPlanResponse>>.Success(
                plans.Select(MapPlan).ToList()));
        }

        private static MealPlanResponse MapPlan(MealPlan plan)
        {
            return new MealPlanResponse(
                plan.Id,
                plan.Name,
                plan.Description,
                plan.TargetCalorieRangeMin,
                plan.TargetCalorieRangeMax,
                plan.MealPlanItems
                    .OrderBy(item => item.DayOfWeek)
                    .ThenBy(item => item.MealTime)
                    .Select(MapPlanItem)
                    .ToList());
        }

        private static MealPlanItemResponse MapPlanItem(MealPlanItem item)
        {
            var meal = item.Meal ?? throw new InvalidOperationException("Meal plan item is missing its meal.");

            return new MealPlanItemResponse(
                item.Id,
                item.DayOfWeek,
                item.MealTime.ToString("HH:mm"),
                new MealPlanMealResponse(
                    meal.Id,
                    meal.Name,
                    meal.Type,
                    meal.Calories,
                    meal.Protein,
                    meal.Carbs,
                    meal.Fats));
        }
    }
}
