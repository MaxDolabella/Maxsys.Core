using Microsoft.Extensions.Caching.Memory;

namespace Maxsys.Core.Caching;

/// <summary>
/// Provides an interface to manage a cache service that wraps IMemoryCache and maintains a thread-safe collection
/// of cache keys. This approach ensures that we can manage cache entries effectively
/// <para/>
/// Source: <see href="https://dotnettutorials.net/lesson/how-to-create-custom-in-memory-cache-in-asp-net-core-web-api/">https://dotnettutorials.net/</see>
/// </summary>
public interface ICacheManager
{
    /// <summary>
    /// Removes all cache entries and clears the tracked keys.
    /// </summary>
    void Clear(Func<string, bool>? predicate = null);

    /// <summary>
    /// Returns all active cache keys (removes expired keys in the process).
    /// </summary>
    IEnumerable<string> GetAllKeys();

    /// <summary>
    /// Removes a cache entry and untracks the key.
    /// </summary>
    /// <param name="key"></param>
    void Remove(string key);

    /// <summary>
    /// Adds a cache entry and tracks its key.
    /// </summary>
    /// <typeparam name="T">Type of the cache value</typeparam>
    /// <param name="key">Cache key</param>
    /// <param name="value">item to be cached</param>
    /// <param name="options">Cache entry options</param>
    void Set<T>(string key, T value, MemoryCacheEntryOptions options);

    /// <summary>
    /// Attempts to retrieve a cache entry and removes expired keys from the dictionary.
    /// </summary>
    /// <typeparam name="T">Type of the cache value</typeparam>
    /// <param name="key">Cache key</param>
    /// <param name="value">Output cache value</param>
    /// <returns>True if the key exists; otherwise, false.</returns>
    bool TryGetValue<T>(string key, out T? value);
}