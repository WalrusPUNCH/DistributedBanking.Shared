using Confluent.Kafka;

namespace Shared.Kafka.Messages;

public record MessageWrapper<TMessage>(TopicPartitionOffset Offset, TMessage Message);