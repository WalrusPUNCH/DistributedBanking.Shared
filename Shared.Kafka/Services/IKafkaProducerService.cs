using Confluent.Kafka;

namespace Shared.Kafka.Services;

public interface IKafkaProducerService<in T> : IDisposable
{
    Task<DeliveryResult<string, string>> ProduceAsync(
        T value, 
        string? key = null,
        IDictionary<string, string>? headers = null, 
        CancellationToken cancellationToken = default);
    
    void Produce(
        T value, 
        string? key = null,
        IDictionary<string, string>? headers = null, 
        Action<DeliveryReport<string, string>>? deliveryHandler = null);
}