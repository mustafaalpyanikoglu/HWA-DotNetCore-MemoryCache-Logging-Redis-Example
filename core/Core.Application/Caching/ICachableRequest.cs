namespace Core.Application.Caching;

/// <summary>
/// Defines a contract for a request that can be cached.
/// </summary>
public interface ICachableRequest
{
    /// <summary>
    /// Gets a value indicating whether the cache should be bypassed for this request.
    /// If true, the request will not use cached data and will go directly to the source.
    /// </summary>
    bool BypassCache { get; }

    /// <summary>
    /// Gets the cache key for the request.
    /// This key is used to identify and access the cached data.
    /// </summary>
    string CacheKey { get; }

    /// <summary>
    /// Gets the cache group key for the request.
    /// This key is used to manage a group of cached data, allowing for group-level cache invalidation.
    /// </summary>
    string? CacheGroupKey { get; }

    /// <summary>
    /// Gets the sliding expiration time for the cached data.
    /// This defines the time period after which the cache will expire if not accessed.
    /// Sliding expiration resets the expiration timer each time the cached data is accessed.
    /// </summary>
    TimeSpan? SlidingExpiration { get; }
}