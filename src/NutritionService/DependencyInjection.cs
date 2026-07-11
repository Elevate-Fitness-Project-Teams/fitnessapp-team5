using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NutritionService.Infrastructure.Authentication;
using NutritionService.Infrastructure.FitnessCalculationEngine;
using NutritionService.Infrastructure.Persistence;

namespace NutritionService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration){
            
            services.AddDbContext<NutritionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsHistoryTable("__EFMigrationsHistory", "NutritionService")
            ));

            services.AddScoped<NutritionDbInitializer>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.Configure<FceOptions>(
                configuration.GetSection(FceOptions.SectionName));

            services.AddHttpClient<IFceClient, FceClient>(
                (sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<FceOptions>>().Value;

                    client.BaseAddress = new Uri(options.BaseUrl);
                });

            return services;
        }
    }
}
