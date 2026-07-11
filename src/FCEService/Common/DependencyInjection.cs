using FCEService.Application.Common.Behaviours;
using FCEService.Application.Common.Interfaces;
using FCEService.Infrastructure.Persistence;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FCEService.Infrastructure.Services;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Http;

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
            
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();

            // Configure JWT Authentication
            var jwtKey = configuration["Jwt:Key"];
            var jwtIssuer = configuration["Jwt:Issuer"];
            var jwtAudience = configuration["Jwt:Audience"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
                    };
                });

            services.AddAuthorization();

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

            // IConnectionMultiplexer — registered ONCE as Singleton
            var redisConnectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
            services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(
                StackExchange.Redis.ConnectionMultiplexer.Connect(redisConnectionString));

            // Redis Distributed Cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "FCEService_";
            });

            // Health Checks — SQL + Redis
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;
            services.AddHealthChecks()
                .AddCheck("sqlserver", () =>
                {
                    try
                    {
                        using var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                        conn.Open();
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT 1";
                        cmd.ExecuteScalar();
                        return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("SQL Server is reachable.");
                    }
                    catch (Exception ex)
                    {
                        return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Unhealthy("SQL Server unreachable.", ex);
                    }
                }, tags: new[] { "db", "ready" })
                .AddCheck<FCEService.Infrastructure.HealthChecks.RedisHealthCheck>("redis", tags: new[] { "redis", "ready" });

            // Rate Limiting — 20 requests per minute PER USER (not global)
            // Anonymous or unauthenticated callers fall back to a stricter limit (5 req/min)
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("fce_api", httpContext =>
                {
                    var key = httpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                              ?? httpContext.Connection?.RemoteIpAddress?.ToString()
                              ?? "anonymous";

                    var isAnonymous = string.Equals(key, "anonymous", StringComparison.OrdinalIgnoreCase)
                                      || !(httpContext.User?.Identity?.IsAuthenticated ?? false);

                    var limiterOptions = new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = isAnonymous ? 5 : 20,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    };

                    return RateLimitPartition.GetSlidingWindowLimiter(key, _ => limiterOptions);
                });
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
