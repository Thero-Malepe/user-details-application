using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using UserDetailsApi.Interfaces;

namespace UserDetailsApi.Services
{
    public class CacheService(IMemoryCache memory, IDistributedCache distributed) : ICacheService
    {
        public async Task<T?> GetCache<T>(string key)
        {
            var memoryObj = memory.Get(key);
            if(memoryObj is not null) return (T)memoryObj;
            var bytes = await distributed.GetAsync(key);
            if (bytes is null) return default;
            var value = JsonSerializer.Deserialize<T>(bytes);
            if (value is not null) memory.Set(key, value, TimeSpan.FromMinutes(1));
            return value;
        }

        public async Task SetCache<T>(string key, T value, TimeSpan ttl)
        {
            memory.Set(
                key,
                value,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = ttl,
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                }
            );

            var bytes = JsonSerializer.Serialize(value);
            await distributed.SetStringAsync(
                key,
                bytes,
                new DistributedCacheEntryOptions 
                { 
                    AbsoluteExpirationRelativeToNow = ttl,
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                }
            );
        }

        public async Task RemoveAsync(string key)
        {
            memory.Remove(key);
            await distributed.RemoveAsync(key);
        }
    }
}
