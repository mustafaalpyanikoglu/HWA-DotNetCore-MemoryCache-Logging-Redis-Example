namespace Core.Application.Caching;

/// <summary>
/// Represents the settings for caching in the application.
/// </summary>
public class CacheSettings
{
    /// <summary>
    /// Gets or sets the sliding expiration time in seconds.
    /// Sliding expiration resets the expiration timer each time the cached data is accessed.
    /// </summary>
    public int SlidingExpiration { get; set; }
}