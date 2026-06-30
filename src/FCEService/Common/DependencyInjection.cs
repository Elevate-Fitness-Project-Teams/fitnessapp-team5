using FCEService.Application.Common.Behaviours;
using FCEService.Application.Common.Interfaces;
using FCEService.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
    
namespace FCEService.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFCEService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register IAppDbContext so Handlers can resolve it via DI
            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            // Register all FluentValidation validators from this assembly
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
            });

            return services;
        }
    }
}


