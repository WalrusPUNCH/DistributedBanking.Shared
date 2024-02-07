namespace Shared.Kafka.Options;

public class KafkaConfigurationBase
{
    public required string Brokers { get; set; }
    public required Dictionary<KafkaTopicSource, string> Connections { get; set; }
}