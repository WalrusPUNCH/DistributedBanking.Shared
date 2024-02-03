namespace Shared.Data.Entities.EndUsers;

public class CustomerEntity : EndUserEntityBase
{
    public required List<string> Accounts { get; set; } = new ();
    public required CustomerPassport Passport { get; set; }
}