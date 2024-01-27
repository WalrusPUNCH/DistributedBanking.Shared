namespace Shared.Kafka.Options;

public class KafkaConfigurationBase
{
    public string Brokers { get; set; }
    public Dictionary<KafkaTopicSource, string> Connections { get; set; }
}