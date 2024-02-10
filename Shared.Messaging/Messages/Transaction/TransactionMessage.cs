using Newtonsoft.Json;
using Shared.Data.Entities.Constants;
using Shared.Messaging.Constants;

namespace Shared.Messaging.Messages.Transaction;

public record TransactionMessage(
    string SourceAccountId,
    string? SourceSecurityCode,
    string? DestinationAccountId,
    TransactionType Type,
    decimal Amount,
    string? Description) : MessageBase
{
    [JsonIgnore]
    public Dictionary<string, string> Headers =>
        string.IsNullOrWhiteSpace(DestinationAccountId)
            ? new Dictionary<string, string>
            {
                { $"{nameof(SourceAccountId)}", SourceAccountId },
                { $"{nameof(Type)}", Type.ToString() }
            }
            : new Dictionary<string, string>
            {
                { $"{nameof(SourceAccountId)}", SourceAccountId },
                { $"{nameof(DestinationAccountId)}", DestinationAccountId },
                { $"{nameof(Type)}", Type.ToString() }
            };
    
    [JsonIgnore]
    public override string ResponseChannelPattern => Channel.TransactionsCreation;
}
