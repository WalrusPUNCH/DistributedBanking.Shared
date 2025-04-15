using Shared.Data.Entities;

namespace Shared.Kafka.Messages;

public record Command(
    string Collection,
    string Id,
    CommandType Operation,
    DateTime CreatedAt,
    Dictionary<string, object?> Payload,
    int Priority = 50);
