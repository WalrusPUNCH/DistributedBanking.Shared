using Newtonsoft.Json;
using Shared.Data.Entities.Constants;
using Shared.Messaging.Constants;

namespace Shared.Messaging.Messages.Account;

public record AccountCreationMessage(
    string CustomerId,
    string Name,
    AccountType Type) : MessageBase
{
    [JsonIgnore]
    public Dictionary<string, string> Headers => new()
    {
        { $"{nameof(CustomerId)}", CustomerId }
    };

    [JsonIgnore]
    public override string ResponseChannelPattern => Channel.AccountCreation;
}