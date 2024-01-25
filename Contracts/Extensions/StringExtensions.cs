namespace Contracts.Extensions;

public static class StringExtensions
{
    public static string NormalizeString(this string value)
    {
        return value.ToUpperInvariant();
    }
}