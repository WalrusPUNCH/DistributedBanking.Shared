using Newtonsoft.Json;
using Shared.Messaging.Constants;

namespace Shared.Messaging.Messages.Identity.Registration;

public record UserRegistrationMessage(
    string FirstName,
    string LastName,
    DateTime BirthDate,
    string PhoneNumber,
    string Email,
    string PasswordHash,
    string Salt,
    Passport Passport) : MessageBase
{
    [JsonIgnore]
    public Dictionary<string, string> Headers => new()
    {
        { $"{nameof(Email)}", Email }
    };
    
    [JsonIgnore]
    public override string ResponseChannelPattern => Channel.CustomersRegistration;
}

public record Passport(
    string DocumentNumber, 
    string Issuer, 
    DateTime IssueDateTime, 
    DateTime ExpirationDateTime);
