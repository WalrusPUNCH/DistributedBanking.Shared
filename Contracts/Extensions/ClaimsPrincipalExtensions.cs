using System.Security.Claims;
using Contracts.Constants;

namespace Contracts.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string Id(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims.First(i => i.Type == ClaimConstants.UserIdClaim).Value;
    }
    
    public static string Email(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims.First(i => i.Type == ClaimTypes.Email).Value;
    }
}