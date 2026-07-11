using NutritionService.Domain.Enums;

namespace NutritionService.Features.MealPlans.GetMealPlans
{
    public sealed record MealPlanResponse(
        Guid Id,
        string Name,
        string Description,
        double TargetCalorieRangeMin,
        double TargetCalorieRangeMax,
        IReadOnlyList<MealPlanItemResponse> Items);

    public sealed record MealPlanItemResponse(
        Guid Id,
        DayOfWeek DayOfWeek,
        string MealTime,
        MealPlanMealResponse Meal);

    public sealed record MealPlanMealResponse(
        Guid Id,
        string Name,
        MealTypeEnum Type,
        double Calories,
        double Protein,
        double Carbs,
        double Fats);
}
