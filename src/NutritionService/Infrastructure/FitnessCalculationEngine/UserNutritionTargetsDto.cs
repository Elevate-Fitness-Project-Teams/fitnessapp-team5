namespace NutritionService.Infrastructure.FitnessCalculationEngine
{
    public sealed class UserNutritionTargetsDto
    {
        public Guid UserId { get; init; }

        public double DailyCalories { get; init; }

        public double ProteinTarget { get; init; }

        public double CarbsTarget { get; init; }

        public double FatsTarget { get; init; }
    }
}
