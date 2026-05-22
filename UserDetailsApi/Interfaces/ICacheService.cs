namespace UserDetailsApi.Interfaces
{
    public interface ICacheService
    {
        Task SetCache<T>(string key, T value, TimeSpan ttl);
        Task<T?> GetCache<T>(string key);
        Task RemoveAsync(string key);
    }
}
