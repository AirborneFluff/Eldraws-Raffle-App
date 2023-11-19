using RaffleApi.Entities;

namespace RaffleApi.Extensions;

public static class HttpContextExtensions
{
    public static Clan GetClan(this HttpContext context)
    {
        if (context.Items["clan"] is not Clan clan)
            throw new Exception("Clan does not exist in this context");

        return clan;
    }
    
    public static AppUser GetUser(this HttpContext context)
    {
        if (context.Items["user"] is not AppUser user)
            throw new Exception("User doesn't exist");

        return user;
    }
    
    public static Raffle GetRaffle(this HttpContext context)
    {
        if (context.Items["raffle"] is not Raffle raffle)
            throw new Exception("Raffle doesn't exist");

        return raffle;
    }
}