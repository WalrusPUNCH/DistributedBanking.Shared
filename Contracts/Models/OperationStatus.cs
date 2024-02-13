using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Contracts.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum OperationStatus
{
    InternalFail,
    BadRequest,
    Processing,
    Success
}