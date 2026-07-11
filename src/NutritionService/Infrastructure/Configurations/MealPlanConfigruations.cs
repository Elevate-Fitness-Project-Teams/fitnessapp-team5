using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Configurations._Common;

namespace NutritionService.Infrastructure.Configurations
{
    public class MealPlanConfigurations : BaseEntityConfiguration<MealPlan>
    {
        public override void Configure(EntityTypeBuilder<MealPlan> builder)
        {
            base.Configure(builder);

            builder.ToTable("MealPlans");

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.TargetCalorieRangeMin)
                .IsRequired();

            builder.Property(m => m.TargetCalorieRangeMax)
                .IsRequired();
        }
    }
}
