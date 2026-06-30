namespace NutritionService.Infrastructure.FitnessCalculationEngine
{
    public class FceOptions
    {
        public const string SectionName = "Fce";
        public string BaseUrl { get; set; } = string.Empty;
        public string TargetPath { get; set; } = string.Empty;
    }
}
