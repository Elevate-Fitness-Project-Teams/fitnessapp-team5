namespace NutritionService.Features.Recommendations.GetRecommendationsByUserId
{
    public static class Endpoint
    {
        public static RouteGroupBuilder MapGetRecommendationsByUserIdEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/recommendations/{userId:guid}", GetRecommendationsByUserIdHandler.HandleAsync)
                .WithName("GetNutritionRecommendationsByUserId");

            return group;
        }
    }
}
