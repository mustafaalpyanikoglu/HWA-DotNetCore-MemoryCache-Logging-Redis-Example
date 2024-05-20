namespace Core.Application.Caching;

/// <summary>
/// Defines a contract for a request that removes cached data.
/// </summary>
public interface ICacheRemoverRequest
{
    /// <summary>
    /// Gets a value indicating whether the cache should be bypassed for this request.
    /// If true, the request will not use cached data and will directly remove the specified cache entries.
    /// </summary>
    bool BypassCache { get; }

    /// <summary>
    /// Gets the cache key for the request.
    /// This key identifies the specific cached data to be removed.
    /// </summary>
    string? CacheKey { get; }

    /// <summary>
    /// Gets the cache group keys for the request.
    /// These keys identify the groups of cached data to be removed.
    /// </summary>
    string[]? CacheGroupKey { get; }
}

