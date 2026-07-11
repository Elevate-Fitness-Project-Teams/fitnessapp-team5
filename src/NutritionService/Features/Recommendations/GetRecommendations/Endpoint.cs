namespace NutritionService.Features.Recommendations.GetRecommendations
{
    public static class Endpoint
    {
        public static RouteGroupBuilder MapGetRecommendationsEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/recommendations", GetRecommendationsHandler.HandleAsync)
                .WithName("GetNutritionRecommendations");

            return group;
        }
    }
}
