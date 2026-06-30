using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Entities;

namespace NutritionService.Infrastructure.Persistence
{
    public class NutritionDbContext : DbContext
    {
        public NutritionDbContext(DbContextOptions<NutritionDbContext> options) : base(options) { }

        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealPlan> MealsPlan { get; set; }
        public DbSet<MealPlanItem> MealPlanItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
        }
    }
}
