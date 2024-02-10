using System.Threading.Tasks.Dataflow;

namespace Shared.Redis.Services;

public interface IRedisSubscriber
{
    Task<IAsyncDisposable> SubAsync<T>(string channel, ActionBlock<T> action);
    
    Task<T> SingleObserveChannel<T>(string channel);

    Task PubAsync<T>(string channel, T value);
    
    Task PubManyAsync<T>(string channel, IReadOnlyDictionary<string, T> values);
    
    IObservable<T> ObserveChannel<T>(string channel);
    
    Task UnSubAsync(string channel);
}