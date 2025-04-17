using Shared.Data.Entities;

namespace Shared.Kafka.Messages;

public record Command(
    string Collection,
    string Id,
    CommandType Operation,
    DateTime CreatedAt,
    object Payload,
    Type PayloadType,
    int Priority = 50);