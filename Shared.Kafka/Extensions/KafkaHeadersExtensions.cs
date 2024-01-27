using Confluent.Kafka;

namespace Shared.Kafka.Extensions;

public static class KafkaHeadersExtensions
{
    public static Headers AddHeader(this Headers headers, string key, string value)
    {
        headers.Add(key, Serializers.Utf8.Serialize(value, SerializationContext.Empty));

        return headers;
    }
}