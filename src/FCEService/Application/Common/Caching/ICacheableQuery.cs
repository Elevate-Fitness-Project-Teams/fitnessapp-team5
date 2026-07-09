using MediatR;

namespace FCEService.Application.Common.Caching;

public interface ICacheableQuery<TResponse> : IRequest<TResponse>
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}
