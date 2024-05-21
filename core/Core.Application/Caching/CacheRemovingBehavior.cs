using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Core.Application.Caching;

/// <summary>
/// A pipeline behavior for handling cache removal of MediatR requests.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICacheRemoverRequest
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CacheRemovingBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheRemovingBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="memoryCache">The memory cache instance.</param>
    /// <param name="logger">The logger instance.</param>
    public CacheRemovingBehavior(
        IMemoryCache memoryCache, 
        ILogger<CacheRemovingBehavior<TRequest, TResponse>> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    /// <summary>
    /// Handles the cache removal logic for the request.
    /// </summary>
    /// <param name="request">The request instance.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response from the next delegate.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        // Proceed with the next delegate if bypass cache is requested
        if (request.BypassCache)
            return await next();

        // Execute the next handler in the pipeline and get the response
        TResponse response = await next();

        // Remove cache entries associated with cache group keys
        if (request.CacheGroupKey != null)
        {
            foreach (var groupKey in request.CacheGroupKey)
            {
                // Try to get the cached group from memory cache
                if (_memoryCache.TryGetValue(groupKey, out HashSet<string>? keysInGroup) && keysInGroup != null)
                {
                    // Remove each key in the group from memory cache
                    foreach (string key in keysInGroup)
                    {
                        _memoryCache.Remove(key);
                        _logger.LogInformation($"Removed Cache -> {key}");
                    }

                    // Remove the group key from memory cache
                    _memoryCache.Remove(groupKey);
                    _logger.LogInformation($"Removed Cache -> {groupKey}");

                    // Remove sliding expiration entry for the cache group
                    _memoryCache.Remove($"{groupKey}SlidingExpiration");
                    _logger.LogInformation($"Removed Cache -> {groupKey}SlidingExpiration");
                }
            }
        }

        // Remove cache entry associated with cache key
        if (request.CacheKey != null)
        {
            _memoryCache.Remove(request.CacheKey);
            _logger.LogInformation($"Removed Cache -> {request.CacheKey}");
        }

        return response;
    }
}
