using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace TheatreProject.PerformanceAPI.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        try
        {
            var cachedValue = await _cache.GetAsync(key);
            
            if (cachedValue != null)
            {
                _logger.LogInformation("Cache hit for key: {Key}", key);
                return JsonSerializer.Deserialize<T>(cachedValue);
            }

            _logger.LogInformation("Cache miss for key: {Key}", key);
            var result = await factory();
            
            try
            {
                var expirationTime = expiration ?? TimeSpan.FromMinutes(5);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expirationTime
                };

                var serializedResult = JsonSerializer.SerializeToUtf8Bytes(result);
                await _cache.SetAsync(key, serializedResult, options);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to set cache for key: {Key}", key);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache operation failed for key: {Key}", key);
            return await factory();
        }
    }
    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}