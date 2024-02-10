using Newtonsoft.Json;
using Shared.Messaging.Constants;

namespace Shared.Messaging.Messages.Account;

public record AccountDeletionMessage(string AccountId) : MessageBase
{
    [JsonIgnore]
    public Dictionary<string, string> Headers => new()
    {
        { $"{nameof(AccountId)}", AccountId }
    };

    [JsonIgnore]
    public override string ResponseChannelPattern => Channel.AccountDeletion;
}



