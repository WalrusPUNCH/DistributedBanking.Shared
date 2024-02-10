using Newtonsoft.Json;
using Shared.Messaging.Constants;

namespace Shared.Messaging.Messages.Identity;

public record CustomerInformationUpdateMessage(
    string CustomerId,
    string DocumentNumber,
    string Issuer,
    DateTime IssueDateTime,
    DateTime ExpirationDateTime) : MessageBase
{
    [JsonIgnore]
    public Dictionary<string, string> Headers => new()
    {
        { $"{nameof(CustomerId)}", CustomerId }
    };
    
    [JsonIgnore]
    public override string ResponseChannelPattern => Channel.CustomersUpdate;
}
