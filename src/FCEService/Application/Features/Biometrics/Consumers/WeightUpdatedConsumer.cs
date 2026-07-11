using FCEService.Application.Features.Biometrics.Commands.RecalculateMetrics;
using FCEService.Application.IntegrationEvents;
using FCEService.Domain.Common.Results;
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
            var error = result.Errors[0];

            // Infrastructure failures (DB timeout, unexpected errors) — throw so MassTransit retries
            if (error.Type == ErrorKind.Unexpected)
            {
                logger.LogError("Infrastructure error while recalculating metrics for user {UserId}. Will retry. Error: {Error}", 
                    context.Message.UserId, error.Description);
                throw new InvalidOperationException(error.Description);
            }

            // Business logic errors (user not found, validation) — log and discard, no retry
            logger.LogWarning("Business error for user {UserId} — message discarded (no retry). Error: [{Code}] {Description}", 
                context.Message.UserId, error.Code, error.Description);
        }
        else
        {
            logger.LogInformation("Successfully recalculated metrics for user {UserId}", context.Message.UserId);
        }
    }
}

