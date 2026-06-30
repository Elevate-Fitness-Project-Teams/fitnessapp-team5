using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Configurations._Common;

namespace NutritionService.Infrastructure.Configurations
{
    public class MealConfigurations : BaseEntityConfiguration<Meal>
    {
        public override void Configure(EntityTypeBuilder<Meal> builder)
        {
            base.Configure(builder);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(m => m.Type)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(m => m.Calories)
                .IsRequired();

            builder.Property(m => m.Protein)
                .IsRequired();

            builder.Property(m => m.Carbs)
                .IsRequired();

            builder.Property(m => m.Fats)
                .IsRequired();

            builder.Property(m => m.IngredientsJson)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(m => m.InstructionsJson)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(m => m.VariationsJson)
                .HasMaxLength(500);

            builder.Property(m => m.AllergensJson)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(m => m.TagsJson)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
