using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Application.Caching;

/// <summary>
/// A pipeline behavior for handling caching of MediatR requests.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICachableRequest
{
    private readonly IDistributedCache _cache;
    private readonly CacheSettings _cacheSettings;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachingBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="cache">The distributed cache instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The configuration instance.</param>
    public CachingBehavior(
        IDistributedCache cache,
        ILogger<CachingBehavior<TRequest, TResponse>> logger,
        IConfiguration configuration
    )
    {
        _cache = cache;
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
        CancellationToken cancellationToken
    )
    {
        // If cache bypass is requested, skip caching and proceed to the next handler.
        if (request.BypassCache)
            return await next();

        TResponse response;
        byte[]? cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);
        // If a response is found in the cache, retrieve it from the cache and return.
        // If no response is found in the cache, process the request and add the result to the cache.
        if (cachedResponse != null)
        {
            response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse))!;
            _logger.LogInformation($"Fetched from Cache -> {request.CacheKey}");
        }
        else
            response = await GetResponseAndAddToCache(request, next, cancellationToken);

        return response;
    }

    /// <summary>
    /// Processes the request and adds the response to the cache.
    /// </summary>
    /// <param name="request">The request instance.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response from the next delegate.</returns>
    private async Task<TResponse> GetResponseAndAddToCache(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        TResponse response = await next();

        TimeSpan slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSettings.SlidingExpiration);
        DistributedCacheEntryOptions cacheOptions = new() { SlidingExpiration = slidingExpiration };

        byte[] serializeData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
        await _cache.SetAsync(request.CacheKey, serializeData, cacheOptions, cancellationToken);
        _logger.LogInformation($"Added to Cache -> {request.CacheKey}");

        // If the request has a cache group key, add the key to the cache group.
        if (request.CacheGroupKey != null)
            await AddCacheKeyToGroup(request, slidingExpiration, cancellationToken);
        return response;
    }
    
    
    /// <summary>
    /// Adds the cache key to the specified cache group.
    /// </summary>
    /// <param name="request">The request instance.</param>
    /// <param name="slidingExpiration">The sliding expiration time.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task AddCacheKeyToGroup(TRequest request, TimeSpan slidingExpiration, CancellationToken cancellationToken)
    {
        byte[]? cacheGroupCache = await _cache.GetAsync(key: request.CacheGroupKey!, cancellationToken);
        HashSet<string> cacheKeysInGroup;
        // If the cache group exists, deserialize the cache keys, add the current request's key if not present, and serialize it back.
        if (cacheGroupCache != null)
        {
            cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroupCache))!;
            // If the cache group does not exist, create a new cache group with the current request's key.
            if (!cacheKeysInGroup.Contains(request.CacheKey))
                cacheKeysInGroup.Add(request.CacheKey);
        }
        else
            cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });
        byte[] newCacheGroupCache = JsonSerializer.SerializeToUtf8Bytes(cacheKeysInGroup);

        byte[]? cacheGroupCacheSlidingExpirationCache = await _cache.GetAsync(
            key: $"{request.CacheGroupKey}SlidingExpiration",
            cancellationToken
        );
        
        // If cache group sliding expiration record does not exist or new sliding expiration is longer than current,
        // update the sliding expiration value.
        int? cacheGroupCacheSlidingExpirationValue = null;
        if (cacheGroupCacheSlidingExpirationCache != null) 
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(
                Encoding.Default.GetString(cacheGroupCacheSlidingExpirationCache)
            );
        
        // Update the sliding expiration value if it does not exist or the new sliding expiration is longer than the current one.
        if (
            cacheGroupCacheSlidingExpirationValue == null
            || slidingExpiration.TotalSeconds > cacheGroupCacheSlidingExpirationValue
        )
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(slidingExpiration.TotalSeconds);
        byte[] serializeCachedGroupSlidingExpirationData = JsonSerializer.SerializeToUtf8Bytes(
            cacheGroupCacheSlidingExpirationValue
        );

        DistributedCacheEntryOptions cacheOptions =
            new() { SlidingExpiration = TimeSpan.FromSeconds(Convert.ToDouble(cacheGroupCacheSlidingExpirationValue)) };

        await _cache.SetAsync(key: request.CacheGroupKey!, newCacheGroupCache, cacheOptions, cancellationToken);
        _logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}");

        await _cache.SetAsync(
            key: $"{request.CacheGroupKey}SlidingExpiration",
            serializeCachedGroupSlidingExpirationData,
            cacheOptions,
            cancellationToken
        );
        _logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}SlidingExpiration");
    }
}