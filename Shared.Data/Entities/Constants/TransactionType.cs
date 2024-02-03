using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shared.Data.Entities.Constants;

[JsonConverter(typeof(StringEnumConverter))]
public enum TransactionType
{
    Deposit,
    Withdrawal,
    Transfer
}