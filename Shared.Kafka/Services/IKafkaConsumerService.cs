namespace Shared.Kafka.Services;

public interface IKafkaConsumerService<TKey, out TValue> : IDisposable
{
    IObservable<TValue> Consume(CancellationToken cancellationToken = default);
        
    IObservable<TValue> Consume(TimeSpan timeout);
}