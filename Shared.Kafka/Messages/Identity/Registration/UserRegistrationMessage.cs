using Newtonsoft.Json;

namespace Shared.Kafka.Messages.Identity.Registration;

public record UserRegistrationMessage(
    string FirstName,
    string LastName,
    DateTime BirtDate,
    string PhoneNumber,
    string Email,
    string PasswordHash,
    string Salt,
    Passport Passport)
{
    [JsonIgnore]
    public Dictionary<string, string> Headers => new()
    {
        { $"{nameof(Email)}", Email }
    };
}

public record Passport(
    string DocumentNumber, 
    string Issuer, 
    DateTime IssueDateTime, 
    DateTime ExpirationDateTime);
