using System.Threading.Tasks.Dataflow;

namespace Shared.Redis.Services;

public interface IRedisSubscriber
{
    Task<IAsyncDisposable> SubAsync<T>(string channel, ActionBlock<T> action);
    
    Task PubAsync<T>(string channel, T value);
    
    Task PubManyAsync<T>(string channel, IReadOnlyDictionary<string, T> values);
    
    IObservable<T> ObserveChannel<T>(string channel);
    
    Task<T> SingleObserveChannel<T>(string channel);

    void Unsub(string channel);
    
    Task UnsubAsync(string channel);
}