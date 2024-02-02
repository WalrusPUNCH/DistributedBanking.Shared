using Newtonsoft.Json;

namespace Shared.Kafka.Messages.Identity;

public record EndUserDeletionMessage(string EndUserId)
{
    [JsonIgnore]
    public Dictionary<string, string> Headers => new()
    {
        { $"{nameof(EndUserId)}", EndUserId }
    };
}