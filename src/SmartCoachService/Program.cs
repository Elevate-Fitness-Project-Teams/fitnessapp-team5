
using Scalar.AspNetCore;
using SmartCoachService.Extensions;

namespace SmartCoachService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();

            // Add services to the container.
            builder.Services.AddAuthorization();


            var app = builder.Build();

            #region Minmal APIs Endpoints

            app.MapSmartCoachEndpoints();

            #endregion

            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "Bulk Authentication Service";
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}
