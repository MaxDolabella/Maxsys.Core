using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace Maxsys.Core.Caching;

public class CacheManager : ICacheManager
{
    private readonly IMemoryCache _memoryCache;
    private readonly ConcurrentDictionary<string, bool> _cacheKeys;

    public CacheManager(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _cacheKeys = new ConcurrentDictionary<string, bool>();
    }

    public void Set<T>(string key, T value, MemoryCacheEntryOptions options)
    {
        _memoryCache.Set(key, value, options);
        _cacheKeys.TryAdd(key, true);
    }

    public bool TryGetValue<T>(string key, out T? value)
    {
        if (_memoryCache.TryGetValue(key, out value))
        {
            return true;
        }

        // Remove the key if it no longer exists in the cache
        _cacheKeys.TryRemove(key, out _);

        value = default;
        return false;
    }

    public void Remove(string key)
    {
        //Remove the key from cache
        _memoryCache.Remove(key);

        // Untrack the key if it exists
        _cacheKeys.TryRemove(key, out _);
    }

    public IEnumerable<string> GetAllKeys()
    {
        return _cacheKeys.Keys;
    }

    public void Clear(Func<string, bool>? predicate = null)
    {
        var items = predicate is null ? _cacheKeys.Keys : _cacheKeys.Keys.Where(predicate);

        foreach (var key in items)
        {
            _memoryCache.Remove(key);
        }
        _cacheKeys.Clear();
    }
}