using Microsoft.EntityFrameworkCore;
using NutritionService.Infrastructure.Persistence;

namespace NutritionService.Common.Extensions
{
    public static class MigrationsExtensions
    {
        public static async Task ApplyMigrationsAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<NutritionDbContext>();
            await context.Database.MigrateAsync();

            var seeder = scope.ServiceProvider.GetService<NutritionDbInitializer>();

            try
            {
                if(seeder is not null)
                    await seeder.SeedAsync();
            }
            catch(Exception)
            {
            }
    }
}}
