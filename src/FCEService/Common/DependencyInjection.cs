using FCEService.Application.Common.Behaviours;
using FCEService.Application.Common.Interfaces;
using FCEService.Infrastructure.Persistence;
using FluentValidation;
using MassTransit;
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

            // Register Domain/Application Services
            services.AddScoped<IPlanAssignmentService, FCEService.Application.Common.Services.PlanAssignmentService>();

            // Register all FluentValidation validators from this assembly
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
            });

            // MassTransit — Consumer Only (FCE consumes events, does NOT publish)
            services.AddMassTransit(x =>
            {
                // No Outbox — FCE does not publish Integration Events
                x.AddConsumer<FCEService.Application.Features.Biometrics.Consumers.WeightUpdatedConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    var host = configuration["MessageBroker:Host"] ?? "localhost";
                    var username = configuration["MessageBroker:Username"] ?? "guest";
                    var password = configuration["MessageBroker:Password"] ?? "guest";

                    cfg.Host(host, "/", h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    // Retry connecting to RabbitMQ on startup — prevents service crash if broker is slow
                    cfg.AutoStart = false;

                    cfg.ConfigureEndpoints(context);
                });
            });

            // Redis Distributed Cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis") ?? "localhost:6379";
                options.InstanceName = "FCEService_";
            });

            // Background Jobs
            services.AddHostedService<FCEService.Infrastructure.BackgroundJobs.MonthlyPlanRefreshJob>();

            return services;
        }
         public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<FCEService.Presentation.ExceptionHandling.GlobalExceptionHandler>();
        return services;
    }

    public static IServiceCollection AddMinimalApiConfiguration(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

        return services;
    }
    }
}
