using System.Text.Json;
using System.Text.Json.Serialization;
using FCEService.Application.Common.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FCEService.Application.Common.Behaviours;

public sealed class CachingBehavior<TRequest, TResponse>(
    IDistributedCache cache,
    ILogger<CachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableQuery<TResponse>
{
    private readonly IDistributedCache _cache = cache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger = logger;
    
    // We need special settings to correctly serialize Result<T> with private fields if necessary, 
    // but System.Text.Json does a good job if constructors are annotated.
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var cacheKey = request.CacheKey;
        
        var cachedResponse = await _cache.GetStringAsync(cacheKey, ct);
        
        if (cachedResponse is not null)
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            var response = JsonSerializer.Deserialize<TResponse>(cachedResponse, _jsonSerializerOptions);
            if (response is not null)
            {
                return response;
            }
        }

        _logger.LogInformation("Cache miss for {CacheKey}", cacheKey);
        
        var result = await next();

        // Fix #6: Only cache SUCCESS results — never cache errors!
        // Caching a failure would serve the error to all users until TTL expires.
        var isError = result is FCEService.Domain.Common.Results.Abstractions.IResult r && !r.IsSuccess;

        if (result is not null && !isError)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = request.Expiration ?? TimeSpan.FromMinutes(5)
            };

            var serializedData = JsonSerializer.Serialize(result, _jsonSerializerOptions);
            await _cache.SetStringAsync(cacheKey, serializedData, options, ct);
        }

        return result;
    }
}
