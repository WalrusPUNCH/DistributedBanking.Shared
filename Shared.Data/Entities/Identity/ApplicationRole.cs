namespace Shared.Data.Entities.Identity;

public class ApplicationRole : BaseEntity
{
    public string Name { get; init; }
    public string NormalizedName { get; init; }
    
    public ApplicationRole(string name)
    {
        Name = name;
        NormalizedName = name.ToUpperInvariant();
    }
}