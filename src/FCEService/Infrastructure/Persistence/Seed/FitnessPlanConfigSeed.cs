using FCEService.Domain.Entities.FitnessPlanConfig;
using FCEService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FCEService.Infrastructure.Persistence.Seed
{
    public static class FitnessPlanConfigSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.FitnessPlanConfigs.AnyAsync())
            {
                return;
            }

            var plan1Result = FitnessPlanConfig.Create(
                "basic_weight_loss",
                "Basic Weight Loss Plan",
                "A beginner-friendly plan for losing weight.",
                Goal.LoseWeight,
                FitnessStatus.Weak,
                1500,
                2000,
                "12 weeks",
                3,
                "Cardio + Light Weights",
                "System");

            var plan2Result = FitnessPlanConfig.Create(
                "advanced_muscle_gain",
                "Advanced Muscle Builder",
                "Heavy lifting for serious muscle gains.",
                Goal.GainWeight,
                FitnessStatus.Hard,
                2800,
                3500,
                "16 weeks",
                5,
                "Hypertrophy",
                "System");

            var plans = new List<FitnessPlanConfig>();
            
            if (plan1Result.IsSuccess) plans.Add(plan1Result.Value);
            if (plan2Result.IsSuccess) plans.Add(plan2Result.Value);

            if (plans.Count > 0)
            {
                context.FitnessPlanConfigs.AddRange(plans);
                await context.SaveChangesAsync();
            }
        }
    }
}
