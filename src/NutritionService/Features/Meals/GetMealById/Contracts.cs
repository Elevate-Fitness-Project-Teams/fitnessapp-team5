using NutritionService.Domain.Enums;

namespace NutritionService.Features.Meals.GetMealById
{
    public sealed record MealDetailsResponse(
        Guid Id,
        string Name,
        MealTypeEnum Type,
        double Calories,
        double Protein,
        double Carbs,
        double Fats,
        IReadOnlyList<string> Ingredients,
        IReadOnlyList<string> Instructions,
        IReadOnlyList<string> Variations,
        IReadOnlyList<string> Allergens,
        IReadOnlyList<string> Tags);
}
