using NutritionService.Features.MealPlans.GetMealPlans;
using NutritionService.Features.MealPlans.GetMealPlansByCalories;
using NutritionService.Features.Meals.GetMealById;
using NutritionService.Features.Recommendations.GetRecommendations;
using NutritionService.Features.Recommendations.GetRecommendationsByUserId;

namespace NutritionService.Common.Extensions
{
    public static class NutritionEndpointExtensions
    {
        public static IEndpointRouteBuilder MapNutritionEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/nutrition")
                .WithTags("Nutrition")
                .RequireAuthorization();

            group.MapGetRecommendationsEndpoint();
            group.MapGetRecommendationsByUserIdEndpoint();
            group.MapGetMealByIdEndpoint();
            group.MapGetMealPlansEndpoint();
            group.MapGetMealPlansByCaloriesEndpoint();

            return app;
        }
    }
}
