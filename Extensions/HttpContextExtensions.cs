using RaffleApi.Entities;

namespace RaffleApi.Extensions;

public static class HttpContextExtensions
{
    public static Clan GetClan(this Microsoft.AspNetCore.Http.HttpContext context)
    {
        var clan = context.Items["clan"] as Clan;
        if (clan == null)
            throw new Exception("Clan does not exist in this context");

        return clan;
    }
    
    public static AppUser GetUser(this Microsoft.AspNetCore.Http.HttpContext context)
    {
        var user = context.Items["user"] as AppUser;
        if (user == null)
            throw new Exception("User doesn't exist");

        return user;
    }
}