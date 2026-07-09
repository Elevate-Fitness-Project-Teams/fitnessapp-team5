using System.Diagnostics;
using FCEService.Domain.Common.Results.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FCEService.Application.Common.Behaviours;

public sealed class LoggingBehaviour<TRequest, TResponse>(
    ILogger<LoggingBehaviour<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        // Fix #13: Log request NAME only — never log request body.
        // Request body may contain PII/health data (weight, height, birthdate).
        logger.LogInformation("Processing Request: {RequestName}", requestName);

        var timer = Stopwatch.StartNew();

        var response = await next();

        timer.Stop();

        if (response is FCEService.Domain.Common.Results.Abstractions.IResult result && !result.IsSuccess)
        {
            logger.LogWarning("Request: {RequestName} failed after {ElapsedMilliseconds} ms. Errors: {@Errors}", 
                requestName, timer.ElapsedMilliseconds, result.Errors);
        }
        else
        {
            logger.LogInformation("Request: {RequestName} completed successfully in {ElapsedMilliseconds} ms.", 
                requestName, timer.ElapsedMilliseconds);
        }

        return response;
    }
}
