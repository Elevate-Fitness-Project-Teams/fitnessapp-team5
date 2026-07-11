using NutritionService.Domain.Enums;

namespace NutritionService.Features.MealPlans.GetMealPlansByCalories
{
    public sealed record MealPlanByCaloriesResponse(
        Guid Id,
        string Name,
        string Description,
        double TargetCalorieRangeMin,
        double TargetCalorieRangeMax,
        IReadOnlyList<MealPlanByCaloriesItemResponse> Items);

    public sealed record MealPlanByCaloriesItemResponse(
        Guid Id,
        DayOfWeek DayOfWeek,
        string MealTime,
        MealPlanByCaloriesMealResponse Meal);

    public sealed record MealPlanByCaloriesMealResponse(
        Guid Id,
        string Name,
        MealTypeEnum Type,
        double Calories,
        double Protein,
        double Carbs,
        double Fats);
}
