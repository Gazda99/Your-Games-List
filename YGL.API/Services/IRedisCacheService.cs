using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace YGL.API.Services; 

public interface IRedisCacheService {
    Task SetCachedResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
    Task<string> GetCachedResponseAsync(string cacheKey);
    Task RemoveKeyAsync(string cacheKey);
}