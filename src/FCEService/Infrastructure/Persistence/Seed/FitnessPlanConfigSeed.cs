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

            // All 9 combinations: 3 Goals × 3 Statuses
            // Every user must always find a matching plan regardless of their calorie profile
            var seedPlans = new[]
            {
                // LoseWeight
                FitnessPlanConfig.Create(Guid.NewGuid(), "Gentle Weight Loss", "Easy deficit for low-activity users.", Goal.LoseWeight, FitnessStatus.Weak, 1200, 1600, "12 weeks", 2, "Walking + Stretching", "System"),
                FitnessPlanConfig.Create(Guid.NewGuid(), "Steady Weight Loss", "Moderate deficit with cardio.", Goal.LoseWeight, FitnessStatus.Normal, 1500, 2000, "12 weeks", 3, "Cardio + Light Weights", "System"),
                FitnessPlanConfig.Create(Guid.NewGuid(), "Aggressive Weight Loss", "High-intensity fat burning.", Goal.LoseWeight, FitnessStatus.Hard, 1800, 2400, "10 weeks", 5, "HIIT + Strength", "System"),

                // GainWeight
                FitnessPlanConfig.Create(Guid.NewGuid(), "Lean Bulk Starter", "Minimal surplus for easy gainers.", Goal.GainWeight, FitnessStatus.Weak, 2200, 2800, "16 weeks", 3, "Compound Movements", "System"),
                FitnessPlanConfig.Create(Guid.NewGuid(), "Classic Bulk", "Moderate surplus for steady muscle gain.", Goal.GainWeight, FitnessStatus.Normal, 2800, 3400, "16 weeks", 4, "Hypertrophy", "System"),
                FitnessPlanConfig.Create(Guid.NewGuid(), "Advanced Muscle Builder", "Heavy lifting for serious gains.", Goal.GainWeight, FitnessStatus.Hard, 3200, 4000, "16 weeks", 5, "Powerlifting + Volume", "System"),

                // GetFitter
                FitnessPlanConfig.Create(Guid.NewGuid(), "Active Lifestyle", "Light activity for general health.", Goal.GetFitter, FitnessStatus.Weak, 1800, 2200, "Ongoing", 2, "Yoga + Walking", "System"),
                FitnessPlanConfig.Create(Guid.NewGuid(), "Maintenance Plan", "Keep your body healthy and fit.", Goal.GetFitter, FitnessStatus.Normal, 2000, 2600, "Ongoing", 4, "CrossFit", "System"),
                FitnessPlanConfig.Create(Guid.NewGuid(), "Athletic Conditioning", "High-output performance training.", Goal.GetFitter, FitnessStatus.Hard, 2600, 3200, "Ongoing", 5, "Athletic + Endurance", "System"),
            };

            var plans = seedPlans
                .Where(r => r.IsSuccess)
                .Select(r => r.Value)
                .ToList();

            if (plans.Count > 0)
            {
                context.FitnessPlanConfigs.AddRange(plans);
                await context.SaveChangesAsync();
            }
        }
    }
}

