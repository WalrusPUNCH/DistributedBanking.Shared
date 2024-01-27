using Confluent.Kafka;

namespace Shared.Kafka.Options;

public class KafkaConsumerConfiguration : KafkaConfigurationBase
{
    public Dictionary<KafkaTopicSource, ConsumerConfig> Consumers { get; set; }  = new();
}