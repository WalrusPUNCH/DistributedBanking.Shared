using Newtonsoft.Json;
using Shared.Data.Entities.Constants;

namespace Shared.Kafka.Messages.Account;

public record AccountCreationMessage(
    string CustomerId,
    string AccountName,
    AccountType Type)
{
    [JsonIgnore]
    public Dictionary<string, string> Headers => new()
    {
        { $"{nameof(CustomerId)}", CustomerId }
    };
}