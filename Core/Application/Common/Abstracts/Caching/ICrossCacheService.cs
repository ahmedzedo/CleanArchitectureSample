using CleanArchitecture.Application.Common.Caching;

namespace CleanArchitecture.Application.Common.Abstracts.Caching;
public interface ICrossCacheService
{
    Task<T?> GetCacheAsync<T>(string cacheKey,
                              CacheStore cacheStore,
                              CrossCacheEntryOption? options = default,
                              CancellationToken cancellationToken = default) where T : class;

    Task SetCacheAsync<T>(string cacheKey,
                          T value,
                          CacheStore cacheStore,
                          CrossCacheEntryOption? options = default,
                          CancellationToken cancellationToken = default) where T : class;

    Task<T> GetOrCreateAsync<T>(string cacheKey,
                                      Func<Task<T>> getResonse,
                                      CrossCacheEntryOption crossCacheEntryOption,
                                      CacheStore cacheStore,
                                      Predicate<T>? validateValue = default,
                                      CancellationToken cancellationToken = default) where T : class, new();

    Task<T> GetOrCreateAsync<T>(string cacheKey,
                                     Func<Task<T>> getResonse,
                                     Predicate<T>? validateValue = default,
                                     CancellationToken cancellationToken = default) where T : class, new();

    Task RemoveAsync(string key, CacheStore cacheStore, CancellationToken cancellationToken = default);

    Task<int> InvalidateAsync(string keyPrefix, CacheStore cacheStore, CancellationToken cancellationToken = default);

    Task InvalidateLocalCacheBySubscibeInvalidateChannel();
}
