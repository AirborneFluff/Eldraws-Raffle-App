using RaffleApi.Entities;

namespace RaffleApi.Extensions;

public static class HttpContextExtensions
{
    public static Clan GetClan(this Microsoft.AspNetCore.Http.HttpContext context)
    {
        if (context.Items["clan"] is not Clan clan)
            throw new Exception("Clan does not exist in this context");

        return clan;
    }
    
    public static AppUser GetUser(this Microsoft.AspNetCore.Http.HttpContext context)
    {
        if (context.Items["user"] is not AppUser user)
            throw new Exception("User doesn't exist");

        return user;
    }
}