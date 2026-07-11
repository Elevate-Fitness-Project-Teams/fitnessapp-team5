using SmartCoachService.Features.GetChatHistory;
using SmartCoachService.Features.SendChatMessage;

namespace SmartCoachService
{
    public static class AllEndpoints
    {
        public static IEndpointRouteBuilder MapSmartCoachEndpoints(
          this IEndpointRouteBuilder app)
        {
            app.MapSendChatMessageEndpoint();
            app.MapGetChatHistoryEndpoint();

            return app;
        }
    }
}
