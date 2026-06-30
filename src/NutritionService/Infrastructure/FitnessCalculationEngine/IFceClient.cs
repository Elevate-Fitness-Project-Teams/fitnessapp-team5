namespace NutritionService.Infrastructure.FitnessCalculationEngine
{
    public interface IFceClient
    {
        Task<UserNutritionTargetsDto?> GetUserTargetsAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
