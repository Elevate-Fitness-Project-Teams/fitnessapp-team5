using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace FCEService.Infrastructure.HealthChecks;

/// <summary>
/// Redis health check using IConnectionMultiplexer injected via constructor DI.
///
/// WHY NOT BuildServiceProvider():
///   BuildServiceProvider() inside ConfigureServices creates a second DI container.
///   Scoped services resolve as Singletons inside it → wrong lifetime, memory leaks.
///   This class is the correct approach: register it as a typed IHealthCheck and
///   let the DI container inject IConnectionMultiplexer normally.
/// </summary>
public sealed class RedisHealthCheck(IConnectionMultiplexer redis) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // PING is the lightest operation Redis supports — minimal overhead on health probes
            var db = redis.GetDatabase();
            await db.PingAsync();
            return HealthCheckResult.Healthy("Redis is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis is unreachable.", ex);
        }
    }
}
