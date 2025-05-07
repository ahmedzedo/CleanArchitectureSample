using System.Reflection;
using CleanArchitecture.Application.Common.Abstracts.Caching;
using CleanArchitecture.Application.Common.Caching;
using CleanArchitecture.Application.Common.Extensions;
using CleanArchitecture.Application.Common.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace CleanArchitecture.Application.Common.Behaviours;

public class CachingBehaviour<TRequest, TResponse> : IRequestResponsePipeline<TRequest, TResponse>
        where TRequest : IAppRequest<TResponse>, ICacheable

{
    #region Dependency
    private readonly ICrossCacheService _crossCacheService;
    private readonly ILogger<CachingBehaviour<TRequest, TResponse>> _logger;
    private CrossCacheEntryOption DefaultCrossCacheEntryOption { get; set; }
    #endregion

    #region Constructor
    public CachingBehaviour(ICrossCacheService crossCache,
                            ILogger<CachingBehaviour<TRequest, TResponse>> logger,
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
        _logger.LogInformation("Caching Behaviour started");
        var cacheAttribute = request.GetType().GetCustomAttribute<CacheAttribute>();

        if (!DefaultCrossCacheEntryOption.IsEnabled || cacheAttribute is null)
        {
            _logger.LogInformation("Caching is disabled");
            _logger.LogInformation("Caching Behaviour Ended");

            return await next();
        }

        return await GetOrCreateCache(request, next, cacheAttribute, cancellationToken);

    }

    private async Task<IResult<TResponse>> GetOrCreateCache(TRequest request,
                                                             MyRequestHandlerDelegate<TResponse> next,
                                                             CacheAttribute cacheAttribute,
                                                             CancellationToken cancellationToken)
    {
        var cacheKey = FormatCacheKey(cacheAttribute.KeyPrefix, request.GetType().Name, request.CahcheKeyIdentifire);
        var crossCacheEntryOption = cacheAttribute.ToCrossCacheEntryOptionsOrDefault(DefaultCrossCacheEntryOption);
        IResult<TResponse>? response;

        try
        {
            async Task<Result<TResponse>> getResonse()
            {
                var result = await next();
                return (result as Result<TResponse>)!;
            }
            response = await _crossCacheService.GetOrCreateAsync(cacheKey,
                                                                 getResonse,
                                                                 crossCacheEntryOption,
                                                                 crossCacheEntryOption.CacheStore,
                                                                 result => result.IsSuccess && result.Error == null,
                                                                 cancellationToken);
            _logger.LogInformation("Cach Behaviour Ended");

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "The caching operation failed, there are exception occurred with {Message}",
                                   ex.Message);
            _logger.LogInformation("Caching Behaviour Ended");
        }

        return await next();
    }

    private static string FormatCacheKey(string keyPrefix,
                                      string requestName,
                                      string cahcheKeyIdentifire) => $"{keyPrefix}:{requestName}:{cahcheKeyIdentifire}";


    #endregion
}
