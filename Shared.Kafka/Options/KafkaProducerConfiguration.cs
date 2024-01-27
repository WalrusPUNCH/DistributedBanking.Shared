using Confluent.Kafka;

namespace Shared.Kafka.Options;

public class KafkaProducerConfiguration : KafkaConfigurationBase
{
    public Dictionary<KafkaTopicSource, ProducerConfig> Producers { get; set; } = new();
}