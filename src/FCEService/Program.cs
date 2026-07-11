using FCEService.Common;
using FCEService.Presentation.Endpoints;
using FCEService.Infrastructure.Persistence;
using FCEService.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext, MediatR, MassTransit, Redis, Health Checks, Rate Limiting, etc.
builder.Services.AddFCEService(builder.Configuration);

// Register Exception Handling via DependencyInjection.cs
builder.Services.AddExceptionHandling();
builder.Services.AddProblemDetails();

// Register Minimal API JSON configuration via DependencyInjection.cs
builder.Services.AddMinimalApiConfiguration();

// Authentication + Authorization are configured inside AddFCEService() in DependencyInjection.cs
// (JWT Bearer scheme + AddAuthorization). Do NOT call AddAuthentication() here again —
// a bare AddAuthentication() without a handler would override the JWT default scheme and cause 401 on all protected endpoints.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// UseHttpsRedirection disabled — Docker runs HTTP only via ASPNETCORE_URLS=http://+:8080

app.UseExceptionHandler();

// Correlation ID — trace requests across services in logs
app.Use(async (context, next) =>
{
    var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                        ?? Guid.NewGuid().ToString();
    context.Response.Headers["X-Correlation-ID"] = correlationId;
    context.Items["CorrelationId"] = correlationId;
    await next();
});

// Rate Limiting middleware — must come before routing
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

// Map Minimal APIs
app.MapBiometricsEndpoints();
app.MapMetricsEndpoints();
app.MapPlanEndpoints();

// Health check endpoints — used by Kubernetes / Docker for liveness & readiness probes
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await context.Response.WriteAsync(result);
    }
});

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

