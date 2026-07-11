using Microsoft.EntityFrameworkCore;
using NutritionService.Common.DTOs;
using NutritionService.Common.Json;
using NutritionService.Common.Pagination;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.FitnessCalculationEngine;
using NutritionService.Infrastructure.Persistence;

namespace NutritionService.Features.Recommendations.GetRecommendationsByUserId
{
    public static class GetRecommendationsByUserIdHandler
    {
        public static async Task<IResult> HandleAsync(
            Guid userId,
            NutritionDbContext dbContext,
            IFceClient fceClient,
            CancellationToken cancellationToken)
        {
            var targets = await fceClient.GetUserTargetsAsync(userId, cancellationToken);

            if (targets is null)
            {
                return Results.NotFound(ApiResponse<UserRecommendationResponse>.Failure("Nutrition targets were not found for this user."));
            }

            var pagedMeals = await dbContext.Meals
                .AsNoTracking()
                .Where(meal => meal.Calories <= targets.DailyCalories)
                .OrderBy(meal => meal.Calories)
                .ToPagedResultAsync(new PaginationRequest
                {
                    Page = 1,
                    PageSize = 20
                }, cancellationToken);

            var response = new UserRecommendationResponse(
                targets,
                new PaginationResult<UserRecommendedMealDto>
                {
                    Items = pagedMeals.Items.Select(MapMeal),
                    TotalCount = pagedMeals.TotalCount,
                    Page = pagedMeals.Page,
                    PageSize = pagedMeals.PageSize
                });

            return Results.Ok(ApiResponse<UserRecommendationResponse>.Success(response));
        }

        private static UserRecommendedMealDto MapMeal(Meal meal)
        {
            return new UserRecommendedMealDto(
                meal.Id,
                meal.Name,
                meal.Type,
                meal.Calories,
                meal.Protein,
                meal.Carbs,
                meal.Fats,
                JsonArrayReader.ReadStringArray(meal.IngredientsJson),
                JsonArrayReader.ReadStringArray(meal.InstructionsJson),
                JsonArrayReader.ReadStringArray(meal.VariationsJson),
                JsonArrayReader.ReadStringArray(meal.AllergensJson),
                JsonArrayReader.ReadStringArray(meal.TagsJson));
        }
    }
}
