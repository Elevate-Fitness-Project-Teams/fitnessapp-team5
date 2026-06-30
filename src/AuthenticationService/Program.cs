using AuthenticationService.Extensions;
using AuthenticationService.Features;
using MassTransit;
using Scalar.AspNetCore;

namespace AuthenticationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            #region Minmal APIs Endpoints

            app.MapAuthenticationEndpoints();

            #endregion

            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "Bulk Authentication Service";
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.MapControllers();

            app.Run();
        }
    }
}
