using Shared.Data.Entities.Constants;

namespace Shared.Data.Entities;

public class TransactionEntity : BaseEntity
{ 
    public required string SourceAccountId { get; set; }
    public string? DestinationAccountId { get; set; }
    public required TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime DateTime { get; set; }
    public string? Description { get; set; }
}