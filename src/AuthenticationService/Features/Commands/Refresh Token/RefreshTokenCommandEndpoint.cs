using AuthenticationService.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Features.Refresh_Token
{
    public static class RefreshTokenCommandEndpoint
    {
        public static IEndpointRouteBuilder MapRefreshTokenEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/auth/refresh-token", [AllowAnonymous] async (
                [FromBody] RefreshTokenRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new RefreshTokenCommand(request.RefreshToken);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            })
            .WithName("RefreshToken")
            .WithSummary("Refresh Token")
            .WithDescription("Refreshes the access token using a valid refresh token.")
            .WithTags("Authentication");

            return app;
        }
    }   
    //[ApiController]
    //[Route("api/v1/auth")]
    //public class RefreshTokenCommandEndpoint : ControllerBase
    //{
    //    [HttpPost("refresh-token")]
    //    [AllowAnonymous]
    //    public async Task<ActionResult<RequestResult<RefreshTokenResponse>>> RefreshToken(
    //        [FromBody] RefreshTokenRequest request,
    //        [FromServices] IMediator mediator,
    //        CancellationToken cancellationToken)
    //    {
    //        var command = new RefreshTokenCommand(request.RefreshToken);
    //        var result = await mediator.Send(command, cancellationToken);
    //        return result;
    //    }
    //}
}
