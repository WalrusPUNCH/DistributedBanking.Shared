using System.Globalization;

namespace Contracts.Models;

public class IdentityOperationResult
{
    public bool Succeeded { get; }
    public IEnumerable<string> Errors { get; }

    private IdentityOperationResult(bool success, IEnumerable<string>? errors = null)
    {
        Succeeded = success;
        Errors = errors ?? new List<string>();
    }

    public static IdentityOperationResult Success { get; } = new(true);
    // todo differentiate between 500 and 400 response codes
    // todo throw exception when 500
    public static IdentityOperationResult BadRequest(params string[] errors)
    {
        return new IdentityOperationResult(false, errors);
    }
    public static IdentityOperationResult Failed(params string[] errors)
    {
        return new IdentityOperationResult(false, errors);
    }
    public static IdentityOperationResult Processing { get; } = new(true, new[] {"Your operation is still processing. Check for it results later"});
    
    public override string ToString()
    {
        return Succeeded ? 
            "Succeeded" 
            : string.Format(CultureInfo.InvariantCulture, "{0} : {1}", "Failed", string.Join(",", Errors.ToList()));
    }
}