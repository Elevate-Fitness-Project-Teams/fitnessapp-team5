using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Configurations._Common;

namespace NutritionService.Infrastructure.Configurations
{
    public class MealPlanItemConfigurations : BaseEntityConfiguration<MealPlanItem>
    {
        public override void Configure(EntityTypeBuilder<MealPlanItem> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.DayOfWeek)
                .HasConversion<string>()
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(x => x.MealTime)
                .HasColumnType("time")
                .IsRequired();

            builder.HasOne(x => x.MealPlan)
                .WithMany(x => x.MealPlanItems)
                .HasForeignKey(x => x.MealPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Meal)
                .WithMany()
                .HasForeignKey(x => x.MealId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
