using Microsoft.AspNetCore.HttpLogging;
using NutritionService;
using NutritionService.Common.Extensions;
using NutritionService.Common.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjection(builder.Configuration);

//Register in HTTP Logging
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
});

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration)
    =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration) //Assigning the project's logging configs to Serilog configs
    .ReadFrom.Services(services);//Read app services and make them availavle to serilog
});


builder.Services.AddOpenApi();

//Register Authentication and Authorization services
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        //TODO: add configuration for IdentityServer:Authority in appsettings.json
        options.Authority = builder.Configuration["IdentityServer:Authority"];
        options.RequireHttpsMetadata = true;
        options.Audience = "nutritionservice";
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandlingMiddleware();
await app.ApplyMigrationsAsync();

app.UseHttpLogging();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.Run();
