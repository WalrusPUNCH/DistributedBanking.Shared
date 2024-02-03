namespace Shared.Data.Entities.Identity;

public class ApplicationUser : BaseEntity
{
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedOn { get; set; }
    public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
    public string EndUserId { get; set; }
}
