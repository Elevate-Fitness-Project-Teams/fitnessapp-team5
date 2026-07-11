using NutritionService.Common.Pagination;
using NutritionService.Domain.Enums;
using NutritionService.Infrastructure.FitnessCalculationEngine;

namespace NutritionService.Features.Recommendations.GetRecommendations
{
    public sealed class GetRecommendationsQuery
    {
        public string? MealType { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public double? MaxCalories { get; set; }
        public double? MinProtein { get; set; }
    }

    public sealed record RecommendationResponse(
        UserNutritionTargetsDto? Targets,
        PaginationResult<RecommendedMealDto> Meals);

    public sealed record RecommendedMealDto(
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
