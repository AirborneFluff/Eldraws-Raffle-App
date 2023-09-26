using RaffleApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace RaffleApi.Data;

public sealed class UserSeed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
        if (users == null) return;

        var roles = new List<IdentityRole>
        {
            new IdentityRole { Name = "Member" },
            new IdentityRole { Name = "Moderator" },
            new IdentityRole { Name = "Admin" },
        };

        foreach (var role in roles)
            await roleManager.CreateAsync(role);

        foreach (var user in users)
        {
            await userManager.CreateAsync(user, "logmein");
            await userManager.AddToRoleAsync(user, "Member");
        }
    }
}