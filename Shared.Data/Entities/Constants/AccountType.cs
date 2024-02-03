using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shared.Data.Entities.Constants;

[JsonConverter(typeof(StringEnumConverter))]
public enum AccountType
{
    Regular,
    Digital,
    GovernmentalSupport
}