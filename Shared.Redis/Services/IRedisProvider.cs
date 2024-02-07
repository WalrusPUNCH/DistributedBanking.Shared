using Newtonsoft.Json.Serialization;

namespace Shared.Redis.Services;

public interface IRedisProvider
{
    Task<T?> GetAsync<T>(string key);
    
    Task SetAsync<T>(string key, T value, TimeSpan ttl);

    Task SetAsync<T>(string key, T value, TimeSpan ttl, IContractResolver contractResolver);

    Task SetManyAsync<T>(IReadOnlyDictionary<string, T> values, TimeSpan ttl);

    Task<long> DeleteManyAsync(IEnumerable<string> keys);
    
    Task<bool> DeleteAsync(string key);
}