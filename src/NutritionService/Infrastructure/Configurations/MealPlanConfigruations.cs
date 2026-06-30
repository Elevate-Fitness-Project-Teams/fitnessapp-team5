using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Configurations._Common;

namespace NutritionService.Infrastructure.Configurations
{
    public class MealPlanConfigruations : BaseEntityConfiguration<MealPlan>
    {
        public override void Configure(EntityTypeBuilder<MealPlan> builder)
        {
            base.Configure(builder);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.TargetCalorieTangeMin)
                .IsRequired();

            builder.Property(m => m.TargetCalorieTangeMax)
                .IsRequired();
        }
    }
}
