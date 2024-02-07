using Shared.Kafka.Messages;

namespace Shared.Kafka.Services;

public interface IKafkaConsumerService<TKey, TValue> : IDisposable
{
    IObservable<MessageWrapper<TValue>> Consume(CancellationToken cancellationToken = default);
        
    IObservable<MessageWrapper<TValue>> Consume(TimeSpan timeout);
}