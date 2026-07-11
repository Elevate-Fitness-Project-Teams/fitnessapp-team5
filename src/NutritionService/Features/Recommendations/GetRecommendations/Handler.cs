using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutritionService.Common.DTOs;
using NutritionService.Common.Json;
using NutritionService.Common.Pagination;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Enums;
using NutritionService.Infrastructure.Authentication;
using NutritionService.Infrastructure.FitnessCalculationEngine;
using NutritionService.Infrastructure.Persistence;

namespace NutritionService.Features.Recommendations.GetRecommendations
{
    public static class GetRecommendationsHandler
    {
        public static async Task<IResult> HandleAsync(
            [AsParameters] GetRecommendationsQuery query,
            NutritionDbContext dbContext,
            IFceClient fceClient,
            ICurrentUserService currentUser,
            CancellationToken cancellationToken)
        {
            if (!TryReadMealType(query.MealType, out var mealType))
            {
                return Results.BadRequest(ApiResponse<RecommendationResponse>.Failure("Invalid mealType."));
            }

            UserNutritionTargetsDto? targets = null;
            if (currentUser.UserId is Guid userId)
            {
                targets = await fceClient.GetUserTargetsAsync(userId, cancellationToken);
            }

            var mealsQuery = dbContext.Meals.AsNoTracking();

            if (mealType is not null)
            {
                mealsQuery = mealsQuery.Where(meal => meal.Type == mealType);
            }

            if (query.MaxCalories is not null)
            {
                mealsQuery = mealsQuery.Where(meal => meal.Calories <= query.MaxCalories);
            }

            if (query.MinProtein is not null)
            {
                mealsQuery = mealsQuery.Where(meal => meal.Protein >= query.MinProtein);
            }

            var pagedMeals = await mealsQuery
                .OrderBy(meal => meal.Name)
                .ToPagedResultAsync(new PaginationRequest
                {
                    Page = query.Page,
                    PageSize = Math.Clamp(query.PageSize, 1, 100)
                }, cancellationToken);

            var response = new RecommendationResponse(
                targets,
                new PaginationResult<RecommendedMealDto>
                {
                    Items = pagedMeals.Items.Select(MapMeal),
                    TotalCount = pagedMeals.TotalCount,
                    Page = pagedMeals.Page,
                    PageSize = pagedMeals.PageSize
                });

            return Results.Ok(ApiResponse<RecommendationResponse>.Success(response));
        }

        private static bool TryReadMealType(string? value, out MealTypeEnum? mealType)
        {
            mealType = null;

            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (!Enum.TryParse<MealTypeEnum>(value, true, out var parsed))
            {
                return false;
            }

            mealType = parsed;
            return true;
        }

        private static RecommendedMealDto MapMeal(Meal meal)
        {
            return new RecommendedMealDto(
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
