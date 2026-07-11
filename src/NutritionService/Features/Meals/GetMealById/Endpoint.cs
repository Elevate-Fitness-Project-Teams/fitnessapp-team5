namespace NutritionService.Features.Meals.GetMealById
{
    public static class Endpoint
    {
        public static RouteGroupBuilder MapGetMealByIdEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/meals/{id:guid}", GetMealByIdHandler.HandleAsync)
                .WithName("GetNutritionMealById");

            return group;
        }
    }
}
