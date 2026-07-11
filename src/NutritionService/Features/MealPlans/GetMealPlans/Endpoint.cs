namespace NutritionService.Features.MealPlans.GetMealPlans
{
    public static class Endpoint
    {
        public static RouteGroupBuilder MapGetMealPlansEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/meal-plans", GetMealPlansHandler.HandleAsync)
                .WithName("GetNutritionMealPlans");

            return group;
        }
    }
}
