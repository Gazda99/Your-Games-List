using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace YGL.API.Services; 

public class RedisCacheService : IRedisCacheService {
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _redisServer;

    private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings {
        Formatting = Formatting.Indented,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public RedisCacheService(IDistributedCache distributedCache, IConnectionMultiplexer redisServer) {
        _distributedCache = distributedCache;
        _redisServer = redisServer;
    }

    public async Task SetCachedResponseAsync(string cacheKey, object response, TimeSpan timeToLive) {
        if (response is null)
            return;

        var serializedResponse = JsonConvert.SerializeObject(response, _serializerSettings);

        await _distributedCache.SetStringAsync(cacheKey, serializedResponse, SetCacheEntryOptions(timeToLive));
    }

    public async Task<string> GetCachedResponseAsync(string cacheKey) {
        var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
        return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
    }

    private static DistributedCacheEntryOptions SetCacheEntryOptions(TimeSpan timeToLive) {
        return new DistributedCacheEntryOptions() {
            AbsoluteExpirationRelativeToNow = timeToLive
        };
    }

    public async Task RemoveKeyAsync(string key) {
        await _distributedCache.RemoveAsync(key);
    }
}