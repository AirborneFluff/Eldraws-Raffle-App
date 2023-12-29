using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    public static async void AggregateEntrantDonations(this WebApplication app)
    {
        var perform = app.Configuration.GetSection("StartupServices").GetValue<bool>("AggregateEntrantDonations");
        if (!perform) return;
        
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider;
        try
        {
            var context = service.GetRequiredService<DataContext>();
            var entrantDonations = await context.Entries
                .GroupBy(entry => entry.EntrantId)
                .Select(g => new
                {
                    EntantId = g.Key,
                    TotalDonations = g.Sum(d => d.Donation)
                }).ToListAsync();

            foreach (var val in entrantDonations)
            {
                var entrant = await context.Entrants.SingleAsync(entrant => entrant.Id == val.EntantId);
                entrant.TotalDonations = val.TotalDonations;
            }

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var logger = service.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred whilst aggregating entrant donations");
        }
    }

    public static async void MigrateRaffleEntryTickets(this WebApplication app)
    {
        var perform = app.Configuration.GetSection("StartupServices").GetValue<bool>("MigrateTicketData");
        if (!perform) return;
        
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider;
        try
        {
            var context = service.GetRequiredService<DataContext>();
            var entries = await context.Entries.ToListAsync();

            foreach (var entry in entries)
            {
                context.Update(entry);
            }
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var logger = service.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred whilst aggregating entrant donations");
        }
    }
}