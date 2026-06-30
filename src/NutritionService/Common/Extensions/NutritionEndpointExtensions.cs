namespace NutritionService.Common.Extensions
{
    public static class NutritionEndpointExtensions
    {
        public static IEndpointRouteBuilder MapNutritionEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/nutrition").WithTags("Nutrition").RequireAuthorization();

            return app;
        }
    }
}
 