using MediatR;

namespace SmartCoachService.Features.GetChatHistory
{
    public static class GetChatHistoryEndpoint
    {
        public static void MapGetChatHistoryEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/smart-coach/history",
                 async (
                     [AsParameters] GetChatHistoryRequest request,
                     IMediator _mediator,
                     CancellationToken cancellationToken) =>
                 {
                     var query = new GetChatHistoryQuery(
                         request.Page,
                         request.PageSize,
                         request.SessionId);
                 
                     var result = await _mediator.Send(query, cancellationToken);
                 
                     return Results.Json(result, statusCode: result.StatusCode);
                 }
                 ).RequireAuthorization()
                  .WithName("GetChatHistory")
                  .WithTags("Chat History")
                  .WithSummary("Retrieve chat history for a specific session")
                  .WithDescription("Retrieve chat history for a specific session");
        }
    }
}
