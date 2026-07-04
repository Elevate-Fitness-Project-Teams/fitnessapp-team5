using Bulk.Shared.Settings;
using Carter;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;
using UserProfileService.Abstractions;
using UserProfileService.Exceptions;
using UserProfileService.Implementation;
using UserProfileService.Implementation.Services;
using UserProfileService.Interfaces;
using UserProfileService.Interfaces.Services;
using UserProfileService.Persistence;
using UserProfileService.Settings;

namespace UserProfileService;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(allowedOrigins!)
            )
        );


        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IFileService, FileService>();

        services.AddHttpContextAccessor();
        services.AddProblemDetails();
        services.AddRabbitMqConfiguration(configuration);
        services.AddAuthenticationConfig(configuration);
        services.AddCarter();

        var assembly = typeof(DependencyInjection).Assembly;

        //Add Mapster
        var mappingConfiguration = TypeAdapterConfig.GlobalSettings;
        mappingConfiguration.Scan(assembly);

        services.AddSingleton<IMapper>(new Mapper(mappingConfiguration));

        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(assembly);

        //Add MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }

    private static IServiceCollection AddRabbitMqConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMq = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>();

        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(DependencyInjection).Assembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMq!.Host, "/", h =>
                {
                    h.Username(rabbitMq.UserName);
                    h.Password(rabbitMq.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key)),
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
            };
        });

        return services;
    }
}
