namespace Shared.Data.Entities.EndUsers;

public class WorkerEntity : EndUserEntityBase
{
    public required string Position { get; set; }
    
    public required Address Address { get; set; }
}

public class Address
{
    public required string Country { get; set; }
    
    public required string Region { get; set; }
    
    public required string City { get; set; }
    
    public required string Street { get; set; }
    
    public required string Building { get; set; }
    
    public required string PostalCode { get; set; }
}