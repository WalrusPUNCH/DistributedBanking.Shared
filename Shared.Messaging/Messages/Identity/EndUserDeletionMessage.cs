using Newtonsoft.Json;
using Shared.Messaging.Constants;

namespace Shared.Messaging.Messages.Identity;

public record EndUserDeletionMessage(string EndUserId) : MessageBase
{
    [JsonIgnore]
    public Dictionary<string, string> Headers => new()
    {
        { $"{nameof(EndUserId)}", EndUserId }
    };
    
    [JsonIgnore]
    public override string ResponseChannelPattern => Channel.UsersDeletion;
}