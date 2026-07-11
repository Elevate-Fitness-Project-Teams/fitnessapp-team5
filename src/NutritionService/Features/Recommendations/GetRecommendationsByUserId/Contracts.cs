using NutritionService.Common.Pagination;
using NutritionService.Domain.Enums;
using NutritionService.Infrastructure.FitnessCalculationEngine;

namespace NutritionService.Features.Recommendations.GetRecommendationsByUserId
{
    public sealed record UserRecommendationResponse(
        UserNutritionTargetsDto Targets,
        PaginationResult<UserRecommendedMealDto> Meals);

    public sealed record UserRecommendedMealDto(
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
