using System.Security.Claims;

namespace RaffleApi.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static string? GetUserId(this ClaimsPrincipal user)
    {
        var guidString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (guidString == null) return null;

        return guidString;
    }
}