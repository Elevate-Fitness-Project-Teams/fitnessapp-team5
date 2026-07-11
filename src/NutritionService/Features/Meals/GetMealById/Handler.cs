using Microsoft.EntityFrameworkCore;
using NutritionService.Common.DTOs;
using NutritionService.Common.Json;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Persistence;

namespace NutritionService.Features.Meals.GetMealById
{
    public static class GetMealByIdHandler
    {
        public static async Task<IResult> HandleAsync(
            Guid id,
            NutritionDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var meal = await dbContext.Meals
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (meal is null)
            {
                return Results.NotFound(ApiResponse<MealDetailsResponse>.Failure("Meal was not found."));
            }

            return Results.Ok(ApiResponse<MealDetailsResponse>.Success(MapMeal(meal)));
        }

        private static MealDetailsResponse MapMeal(Meal meal)
        {
            return new MealDetailsResponse(
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
