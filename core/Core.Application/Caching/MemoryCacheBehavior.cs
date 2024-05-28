using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Application.Caching;

/// <summary>
/// A pipeline behavior for handling caching of MediatR requests using memory cache.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class MemoryCacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICachableRequest
{
    private readonly IMemoryCache _memoryCache;
    private readonly CacheSettings _cacheSettings;
    private readonly ILogger<MemoryCacheBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryCacheBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="memoryCache">The memory cache instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The configuration instance.</param>
    public MemoryCacheBehavior(
        IMemoryCache memoryCache,
        ILogger<MemoryCacheBehavior<TRequest, TResponse>> logger,
        IConfiguration configuration)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// Handles the caching logic for the request.
    /// </summary>
    /// <param name="request">The request instance.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response from the cache or the next delegate.</returns>
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        // Check if the cache should be bypassed
        if (request.BypassCache) return await next();

        // Try to get the response from the memory cache
        if (_memoryCache.TryGetValue(request.CacheKey, out TResponse response))
        {
            _logger.LogInformation($"Fetched from MemoryCache -> {request.CacheKey}");
            return response; 
        }

        response = await next();

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(request.SlidingExpiration ?? TimeSpan.FromSeconds(_cacheSettings.SlidingExpiration));

        _memoryCache.Set(request.CacheKey, response, cacheEntryOptions);
        _logger.LogInformation($"Added to MemoryCache -> {request.CacheKey}");

        return response;
    }
}
