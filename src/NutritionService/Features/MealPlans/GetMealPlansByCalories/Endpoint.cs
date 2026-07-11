namespace NutritionService.Features.MealPlans.GetMealPlansByCalories
{
    public static class Endpoint
    {
        public static RouteGroupBuilder MapGetMealPlansByCaloriesEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/meal-plans/by-calories", GetMealPlansByCaloriesHandler.HandleAsync)
                .WithName("GetNutritionMealPlansByCalories");

            return group;
        }
    }
}
