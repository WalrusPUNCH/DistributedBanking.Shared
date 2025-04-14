namespace Shared.Kafka.Messages;

public record Command(
    string Database,
    string Collection,
    string Id,
    DateTime CreatedAt,
    object Payload,
    int Priority = 50);
