using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace YGL.API.Services {
public class RedisCacheService : IRedisCacheService {
    private readonly IDistributedCache _distributedCache;

    public RedisCacheService(IDistributedCache distributedCache) {
        _distributedCache = distributedCache;
    }

    public async Task SetCachedResponseAsync(string cacheKey, object response, TimeSpan timeToLive) {
        if (response is null)
            return;

        string serializedResponse = JsonConvert.SerializeObject(response, Formatting.Indented);

        await _distributedCache.SetStringAsync(cacheKey, serializedResponse, SetCacheEntryOptions(timeToLive));
    }

    public async Task<string> GetCachedResponseAsync(string cacheKey) {
        string cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
        return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
    }

    private static DistributedCacheEntryOptions SetCacheEntryOptions(TimeSpan timeToLive) {
        return new DistributedCacheEntryOptions() {
            AbsoluteExpirationRelativeToNow = timeToLive
        };
    }
}
}