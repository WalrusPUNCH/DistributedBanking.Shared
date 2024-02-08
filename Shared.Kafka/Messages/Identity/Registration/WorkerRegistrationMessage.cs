namespace Shared.Kafka.Messages.Identity.Registration;

public record WorkerRegistrationMessage(
    string FirstName,
    string LastName,
    DateTime BirthDate,
    string PhoneNumber,
    string Email,
    string PasswordHash,
    string Salt,
    string Role,
    string Position,
    Passport Passport,
    Address Address) 
    : UserRegistrationMessage(FirstName, LastName, BirthDate, PhoneNumber, Email, PasswordHash, Salt, Passport);
    
public record Address(string Country, string Region, string City, string Street, string Building, string PostalCode);
