using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Core.Application.Caching
{
    /// <summary>
    /// A pipeline behavior for handling cache removal of MediatR requests.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICacheRemoverRequest
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheRemovingBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheRemovingBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="cache">The distributed cache instance.</param>
        /// <param name="logger">The logger instance.</param>
        public CacheRemovingBehavior(IDistributedCache cache, ILogger<CacheRemovingBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
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
            if (request.BypassCache)
                return await next();

            TResponse response = await next();

            // Remove cache entries associated with cache group keys
            if (request.CacheGroupKey != null)
            {
                foreach (var groupKey in request.CacheGroupKey)
                {
                    byte[]? cachedGroup = await _cache.GetAsync(groupKey, cancellationToken);
                    if (cachedGroup != null)
                    {
                        HashSet<string> keysInGroup = JsonSerializer.Deserialize<HashSet<string>>(
                            Encoding.Default.GetString(cachedGroup)
                        )!;
                        foreach (string key in keysInGroup)
                        {
                            await _cache.RemoveAsync(key, cancellationToken);
                            _logger.LogInformation($"Removed Cache -> {key}");
                        }

                        await _cache.RemoveAsync(groupKey, cancellationToken);
                        _logger.LogInformation($"Removed Cache -> {groupKey}");

                        // Remove sliding expiration entry for the cache group
                        await _cache.RemoveAsync(key: $"{groupKey}SlidingExpiration", cancellationToken);
                        _logger.LogInformation($"Removed Cache -> {groupKey}SlidingExpiration");
                    }
                }
            }

            // Remove cache entry associated with cache key
            if (request.CacheKey != null)
            {
                await _cache.RemoveAsync(request.CacheKey, cancellationToken);
                _logger.LogInformation($"Removed Cache -> {request.CacheKey}");
            }

            return response;
        }
    }
}
