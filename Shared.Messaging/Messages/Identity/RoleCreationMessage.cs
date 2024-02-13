using Newtonsoft.Json;
using Shared.Messaging.Constants;

namespace Shared.Messaging.Messages.Identity;

public record RoleCreationMessage(string Name) : MessageBase
{
    [JsonIgnore]
    public override string ResponseChannelPattern => Channel.RoleCreation;
}