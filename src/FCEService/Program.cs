using FCEService.Common;
using FCEService.Presentation.Endpoints;
using FCEService.Infrastructure.Persistence;
using FCEService.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext and MediatR (resolves IPublisher for migrations)
builder.Services.AddFCEService(builder.Configuration);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Map Minimal APIs
app.MapBiometricsEndpoints();
app.MapMetricsEndpoints();
app.MapPlanEndpoints();

// Auto-migrate and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await FitnessPlanConfigSeed.SeedAsync(db);
}

app.Run();
