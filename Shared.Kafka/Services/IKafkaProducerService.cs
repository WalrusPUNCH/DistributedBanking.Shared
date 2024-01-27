using Confluent.Kafka;

namespace Shared.Kafka.Services;

public interface IKafkaProducerService<in T> : IDisposable
{
    Task<DeliveryResult<string, string>> ProduceAsync(
        T model, 
        IDictionary<string, string>? headers = null, 
        CancellationToken cancellationToken = default);
    
    void Produce(
        T model, 
        IDictionary<string, string>? headers = null, 
        Action<DeliveryReport<string, string>>? deliveryHandler = null);
}