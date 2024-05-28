using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Application.Caching;

/// <summary>
/// A pipeline behavior for handling caching of MediatR requests using memory cache.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class DistributedCacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICachableRequest
{
    private readonly IDistributedCache _distributedCache;
    private readonly CacheSettings _cacheSettings;
    private readonly ILogger<DistributedCacheBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DistributedCacheBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="distributedCache"></param>
    public DistributedCacheBehavior(
        ILogger<DistributedCacheBehavior<TRequest, TResponse>> logger,
        IConfiguration configuration, 
        IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
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
        
        TResponse response;
        var cachedResponse = await _distributedCache.GetAsync(request.CacheKey, cancellationToken);
        if (cachedResponse != null)
        {
            response = JsonSerializer.Deserialize<TResponse>(Encoding.UTF8.GetString(cachedResponse))!;
            _logger.LogInformation($"Fetched from Cache -> {request.CacheKey}");
        }
        else
            response = await GetResponseAndAddToCache(request, next, cancellationToken);
        return response;
    }

    private async Task<TResponse> GetResponseAndAddToCache(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var response = await next();

        var slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSettings.SlidingExpiration);
        DistributedCacheEntryOptions cacheEntryOptions = new() { SlidingExpiration = slidingExpiration };

        var serializeData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
        var cacheTasks = new List<Task>
        {
            _distributedCache.SetAsync(request.CacheKey, serializeData, cacheEntryOptions, cancellationToken)
        };

        _logger.LogInformation($"Added to Cache => {request.CacheKey}");

        if (request.CacheGroupKey != null)
            cacheTasks.Add(AddCacheKeyToGroup(request, slidingExpiration, cancellationToken));

        await Task.WhenAll(cacheTasks);

        return response;
    }


    private async Task AddCacheKeyToGroup(
        TRequest request,
        TimeSpan slidingExpiration,
        CancellationToken cancellationToken
    )
    {
        // Retrieve and update the cache group with the new cache key
        var cacheKeysInGroup = await GetAndUpdateCacheGroupAsync(request.CacheGroupKey!, request.CacheKey, cancellationToken);

        var newCacheGroupCache = JsonSerializer.SerializeToUtf8Bytes(cacheKeysInGroup);
        // Retrieve the sliding expiration value for the cache group
        var cacheGroupCacheSlidingExpirationValue = await GetAndUpdateSlidingExpirationValueAsync(request.CacheGroupKey!, slidingExpiration, cancellationToken);
        
        var serializeCachedGroupSlidingExpirationData = 
            JsonSerializer.SerializeToUtf8Bytes(cacheGroupCacheSlidingExpirationValue);
        
        DistributedCacheEntryOptions cacheOptions =
            new() { SlidingExpiration = TimeSpan.FromSeconds(Convert.ToDouble(cacheGroupCacheSlidingExpirationValue)) };
        
        await _distributedCache.SetAsync(key: request.CacheGroupKey!, newCacheGroupCache, cacheOptions, cancellationToken);
        _logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}");

        await _distributedCache.SetAsync(
            key: $"{request.CacheGroupKey}SlidingExpiration",
            serializeCachedGroupSlidingExpirationData,
            cacheOptions,
            cancellationToken
        );
        _logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}SlidingExpiration");
    }
    
    /// <summary>
    /// Retrieves the cache group and updates it with the new cache key.
    /// </summary>
    /// <param name="cacheGroupKey">The cache group key.</param>
    /// <param name="cacheKey">The cache key to add to the group.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated cache group.</returns>
    private async Task<HashSet<string>> GetAndUpdateCacheGroupAsync(string cacheGroupKey, string cacheKey, CancellationToken cancellationToken)
    {
        // Try to get the existing cache group from the cache.
        var cacheGroupCache = await _distributedCache.GetAsync(cacheGroupKey, cancellationToken);
        HashSet<string> cacheKeysInGroup;

        // If the cache group exists, deserialize it.
        if (cacheGroupCache != null)
        {
            cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.UTF8.GetString(cacheGroupCache))!;
            // If the cache group does not already contain the cache key, add it.
            if (!cacheKeysInGroup.Contains(cacheKey)) cacheKeysInGroup.Add(cacheKey);
        }
        else cacheKeysInGroup = [cacheKey];
        return cacheKeysInGroup;
    }
    
    /// <summary>
    /// Retrieves the sliding expiration value for a cache group and updates if necessary.
    /// </summary>
    /// <param name="cacheGroupKey">The cache group key.</param>
    /// <param name="slidingExpiration">The sliding expiration time.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated sliding expiration value.</returns>
    private async Task<int?> GetAndUpdateSlidingExpirationValueAsync(string cacheGroupKey, TimeSpan slidingExpiration, CancellationToken cancellationToken)
    {
        var cacheGroupCacheSlidingExpirationCache =
            await _distributedCache.GetAsync(
                key: $"{cacheGroupKey}SlidingExpiration",
                cancellationToken);

        int? cacheGroupCacheSlidingExpirationValue = null;

        if (cacheGroupCacheSlidingExpirationCache != null)
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(Encoding.Default.GetString(cacheGroupCacheSlidingExpirationCache));

        if (cacheGroupCacheSlidingExpirationValue == null || slidingExpiration.TotalSeconds > cacheGroupCacheSlidingExpirationValue)
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(slidingExpiration.TotalSeconds);

        return cacheGroupCacheSlidingExpirationValue;
    }
}
