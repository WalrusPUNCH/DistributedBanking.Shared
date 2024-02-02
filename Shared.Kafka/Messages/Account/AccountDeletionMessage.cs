using Newtonsoft.Json;

namespace Shared.Kafka.Messages.Account;

public record AccountDeletionMessage(string AccountId)
{
    [JsonIgnore]
    public Dictionary<string, string> Headers => new()
    {
        { $"{nameof(AccountId)}", AccountId }
    };
}



