using AuthenticationService.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AuthenticationService.Features.Commands.Register;

public static class RegisterCommandEndpoint
{
    public static IEndpointRouteBuilder MapRegisterEndpoint(
        this IEndpointRouteBuilder app)
    {

        app.MapPost("/api/v1/auth/register", [AllowAnonymous] async (
                                          RegisterRequest request,
                                          IMediator mediator,
                                          CancellationToken cancellationToken) =>
            {
                var command = new RegisterCommand(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password,
                    request.PhoneNumber);

                var result = await mediator.Send(command, cancellationToken);

                return result;
            })
            .WithName("Register")
            .WithTags("Authentication")
            .WithSummary("Register")
            .WithDescription("Registers a new user.");
        
        return app;
    }
}
//[Route("api/v1/auth")]
//[ApiController]
//public class RegisterCommandEndpoint(IMediator _mediator) : ControllerBase
//{
//    [AllowAnonymous]
//    [HttpPost("register")]
//    public async Task<ActionResult<RequestResult<RegisterResponse>>> Register(
//        [FromBody] RegisterRequest request, CancellationToken cancellationToken)
//    {
//        var command = new RegisterCommand(
//            request.FirstName,
//            request.LastName,
//            request.Email,
//            request.Password,
//            request.PhoneNumber);

//        var result = await _mediator.Send(command, cancellationToken);

//        return result;
//    }
//}