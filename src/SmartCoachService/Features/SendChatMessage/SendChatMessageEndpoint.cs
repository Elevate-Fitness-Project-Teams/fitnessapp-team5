using MediatR;

namespace SmartCoachService.Features.SendChatMessage
{
    public static class SendChatMessageEndpoint
    {
        public static IEndpointRouteBuilder MapSendChatMessageEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/smart-coach/chat",
                async (SendChatMessageRequest request,
                       IMediator _mediator,
                       CancellationToken cancellationToken) =>
                {
                    var command = new SendChatMessageCommand(request.Message, request.SessionId);
                    var result = await _mediator.Send(command, cancellationToken);

                    return result;
                })
            .RequireAuthorization()
            .WithName("SendChatMessage")
            .WithTags("Smart Coach")
            .WithSummary("Send a message to the AI coach")
            .WithDescription("Starts a new chat session or continues an existing one.");

            return app;
        }
    }
}
