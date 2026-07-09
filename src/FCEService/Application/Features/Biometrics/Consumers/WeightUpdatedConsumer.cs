using FCEService.Application.Features.Biometrics.Commands.RecalculateMetrics;
using FCEService.Application.IntegrationEvents;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FCEService.Application.Features.Biometrics.Consumers;

public class WeightUpdatedConsumer(IMediator mediator, ILogger<WeightUpdatedConsumer> logger) 
    : IConsumer<WeightUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<WeightUpdatedIntegrationEvent> context)
    {
        logger.LogInformation("Received WeightUpdatedIntegrationEvent for user {UserId} with new weight {NewWeight}", 
            context.Message.UserId, context.Message.NewWeight);

        var command = new RecalculateMetricsCommand(context.Message.UserId, context.Message.NewWeight);
        
        var result = await mediator.Send(command, context.CancellationToken);

        if (result.IsError)
        {
            logger.LogError("Failed to recalculate metrics for user {UserId}. Error: {Error}", 
                context.Message.UserId, result.Errors[0].Description);
            
            // Optionally, we could throw an exception to let MassTransit retry it,
            // but for now we'll just log the error to avoid poison queues if it's a validation error.
        }
        else
        {
            logger.LogInformation("Successfully recalculated metrics for user {UserId}", context.Message.UserId);
        }
    }
}
