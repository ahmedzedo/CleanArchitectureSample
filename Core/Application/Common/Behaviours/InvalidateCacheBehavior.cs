using CleanArchitecture.Application.Common.Abstracts.Caching;
using CleanArchitecture.Application.Common.Caching;
using CleanArchitecture.Application.Common.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
namespace CleanArchitecture.Application.Common.Behaviours
{
    public class InvalidateCacheBehavior<TRequest, TResponse> : IRequestResponsePipeline<TRequest, TResponse>
        where TRequest : IAppRequest<TResponse>, ICacheInvalidator
    {
        #region Dependency
        private readonly ICrossCacheService _crossCacheService;
        private readonly ILogger<InvalidateCacheBehavior<TRequest, TResponse>> _logger;
        private CrossCacheEntryOption DefaultCrossCacheEntryOption { get; set; }
        #endregion

        #region Constructor
        public InvalidateCacheBehavior(ICrossCacheService crossCache,
                                ILogger<InvalidateCacheBehavior<TRequest, TResponse>> logger,
                                IOptions<CrossCacheEntryOption> options)
        {
            _crossCacheService = crossCache;
            _logger = logger;
            DefaultCrossCacheEntryOption = options.Value;

        }
        #endregion

        #region Handle
        public async Task<IResult<TResponse>> Handle(TRequest request,
                                                      MyRequestHandlerDelegate<TResponse> next,
                                                      CancellationToken cancellationToken)
        {
            _logger.LogInformation("Invalidate Caching Behaviour started");
            var invalidCacheAttribute = request.GetType().GetCustomAttribute<InvalidCacheAttribute>();

            if (!DefaultCrossCacheEntryOption.IsEnabled || invalidCacheAttribute is null)
            {
                _logger.LogInformation("Caching is disabled");
                _logger.LogInformation("Invalidate Caching Behaviour Ended");
            }
            else
            {
                await InvalidateCache(invalidCacheAttribute, cancellationToken);
            }

            return await next();
        }

        private async Task InvalidateCache(InvalidCacheAttribute? invalidCacheAttribute, CancellationToken cancellationToken)
        {
            try
            {
                await _crossCacheService.InvalidateCacheAsync($"{invalidCacheAttribute!.KeyPrefix}:",
                                                              invalidCacheAttribute.CacheStore,
                                                              cancellationToken);
                _logger.LogInformation("{KeyPrefix} become invaild in cache. will remove with all related keys from cache.",
                                       invalidCacheAttribute.KeyPrefix);
                _logger.LogInformation("Invalidate Cach Behaviour Ended");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "The caching operation failed, there are exception occurred with {Message}",
                                       ex.Message);
                _logger.LogInformation("Invalidate Caching Behaviour Ended");
            }
        }
        #endregion
    }
}
