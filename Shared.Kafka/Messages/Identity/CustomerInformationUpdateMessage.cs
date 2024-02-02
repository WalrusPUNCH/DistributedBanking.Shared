using Newtonsoft.Json;

namespace Shared.Kafka.Messages.Identity;

public record CustomerInformationUpdateMessage(
    string CustomerId,
    string DocumentNumber,
    string Issuer,
    DateTime IssueDateTime,
    DateTime ExpirationDateTime)
{
    [JsonIgnore]
    public Dictionary<string, string> Headers => new()
    {
        { $"{nameof(CustomerId)}", CustomerId }
    };
}
