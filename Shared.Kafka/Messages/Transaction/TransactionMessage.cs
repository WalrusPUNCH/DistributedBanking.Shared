using Newtonsoft.Json;

namespace Shared.Kafka.Messages.Transaction;

public record TransactionMessage(
    string SourceAccountId,
    string? SourceSecurityCode,
    string? DestinationAccountId,
    decimal Amount,
    string? Description)
{
    [JsonIgnore]
    public Dictionary<string, string> Headers =>
        string.IsNullOrWhiteSpace(DestinationAccountId)
            ? new Dictionary<string, string>
            {
                { $"{nameof(SourceAccountId)}", SourceAccountId },
            }
            : new Dictionary<string, string>
            {
                { $"{nameof(SourceAccountId)}", SourceAccountId },
                { $"{nameof(DestinationAccountId)}", DestinationAccountId }
            };
}
