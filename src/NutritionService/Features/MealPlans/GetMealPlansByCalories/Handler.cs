using Microsoft.EntityFrameworkCore;
using NutritionService.Common.DTOs;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Persistence;

namespace NutritionService.Features.MealPlans.GetMealPlansByCalories
{
    public static class GetMealPlansByCaloriesHandler
    {
        public static async Task<IResult> HandleAsync(
            double calories,
            NutritionDbContext dbContext,
            CancellationToken cancellationToken)
        {
            if (calories <= 0)
            {
                return Results.BadRequest(ApiResponse<IReadOnlyList<MealPlanByCaloriesResponse>>.Failure("Calories must be greater than zero."));
            }

            var plans = await dbContext.MealPlans
                .AsNoTracking()
                .Include(plan => plan.MealPlanItems)
                .ThenInclude(item => item.Meal)
                .Where(plan => plan.TargetCalorieRangeMin <= calories && calories <= plan.TargetCalorieRangeMax)
                .OrderBy(plan => plan.TargetCalorieRangeMin)
                .ThenBy(plan => plan.Name)
                .ToListAsync(cancellationToken);

            return Results.Ok(ApiResponse<IReadOnlyList<MealPlanByCaloriesResponse>>.Success(
                plans.Select(MapPlan).ToList()));
        }

        private static MealPlanByCaloriesResponse MapPlan(MealPlan plan)
        {
            return new MealPlanByCaloriesResponse(
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

        private static MealPlanByCaloriesItemResponse MapPlanItem(MealPlanItem item)
        {
            var meal = item.Meal ?? throw new InvalidOperationException("Meal plan item is missing its meal.");

            return new MealPlanByCaloriesItemResponse(
                item.Id,
                item.DayOfWeek,
                item.MealTime.ToString("HH:mm"),
                new MealPlanByCaloriesMealResponse(
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
