using Microsoft.AspNetCore.Identity;
using RaffleApi.Data;
using RaffleApi.Entities;

namespace RaffleApi.Extensions;

public static class WebApplicationExtensions
{
    public static async void SeedApplicationUsers(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider;
        try
        {
            var userManager = service.GetRequiredService<UserManager<AppUser>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            await UserSeed.SeedUsers(userManager, roleManager);
        }
        catch (Exception ex)
        {
            var logger = service.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during migration");
        }
    }
}