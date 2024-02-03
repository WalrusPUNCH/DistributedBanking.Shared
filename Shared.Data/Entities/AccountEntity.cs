using Shared.Data.Entities.Constants;

namespace Shared.Data.Entities;

public class AccountEntity : BaseEntity
{
    public required string Name { get; set; }
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
    public DateTime ExpirationDate { get; set; }
    public required string SecurityCode { get; set; }
    public string? Owner { get; set; }
    public DateTime CreatedAt { get; set; }
}