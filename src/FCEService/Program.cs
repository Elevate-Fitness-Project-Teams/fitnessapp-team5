using FCEService.Common;
using FCEService.Presentation.Endpoints;
using FCEService.Infrastructure.Persistence;
using FCEService.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext, MediatR, MassTransit, Redis, etc.
builder.Services.AddFCEService(builder.Configuration);

// Register Exception Handling via DependencyInjection.cs
builder.Services.AddExceptionHandling();
builder.Services.AddProblemDetails();

// Register Minimal API JSON configuration via DependencyInjection.cs
builder.Services.AddMinimalApiConfiguration();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// UseHttpsRedirection disabled — Docker runs HTTP only via ASPNETCORE_URLS=http://+:8080

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

// Map Minimal APIs
app.MapBiometricsEndpoints();
app.MapMetricsEndpoints();
app.MapPlanEndpoints();

// Auto-migrate and seed data on startup (Development ONLY)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        await FitnessPlanConfigSeed.SeedAsync(db);
    }
}

app.Run();
