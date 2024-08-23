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
                .Where(entry => !entry.Complimentary)
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

    public static async void AggregateRaffleDonations(this WebApplication app)
    {
        var perform = app.Configuration.GetSection("StartupServices").GetValue<bool>("AggregateRaffleDonations");
        if (!perform) return;
        
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider;
        try
        {
            var context = service.GetRequiredService<DataContext>();
            var donations = await context.Entries
                .Where(entry => !entry.Complimentary)
                .GroupBy(entry => entry.RaffleId)
                .Select(g => new
                {
                    RaffleId = g.Key,
                    TotalDonations = g.Sum(d => d.Donation)
                }).ToListAsync();

            foreach (var val in donations)
            {
                var raffle = await context.Raffles.SingleAsync(raffle => raffle.Id == val.RaffleId);
                raffle.TotalDonations = val.TotalDonations;
            }

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var logger = service.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred whilst aggregating raffle donations");
        }
    }

    public static async void AggregateRaffleTickets(this WebApplication app)
    {
        var perform = app.Configuration.GetSection("StartupServices").GetValue<bool>("AggregateRaffleTickets");
        if (!perform) return;
        
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider;
        try
        {
            var context = service.GetRequiredService<DataContext>();
            var donations = await context.Entries
                .GroupBy(entry => entry.RaffleId)
                .Select(g => new
                {
                    RaffleId = g.Key,
                    HighestTicket = g.Max(e => e.HighTicket)
                }).ToListAsync();

            foreach (var val in donations)
            {
                var raffle = await context.Raffles.SingleAsync(raffle => raffle.Id == val.RaffleId);
                raffle.TotalTickets = val.HighestTicket;
            }

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var logger = service.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred whilst aggregating raffle tickets");
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
            logger.LogError(ex, "An error occurred whilst migrating raffle entry tickets");
        }
    }

    public static async void MigratePrizeWinners(this WebApplication app)
    {
        var perform = app.Configuration.GetSection("StartupServices").GetValue<bool>("MigratePrizeWinners");
        if (!perform) return;
        
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider;
        try
        {
            var context = service.GetRequiredService<DataContext>();
            var prizes = await context.Prizes
                .Where(p => p.WinningTicketNumber != null)
                .ToListAsync();

            foreach (var prize in prizes)
            {
                var winner = await context.Entries
                    .Where(entry => entry.RaffleId == prize.RaffleId)
                    .FirstOrDefaultAsync(e => e.LowTicket <= prize.WinningTicketNumber && e.HighTicket >= prize.WinningTicketNumber);
                prize.WinnerId = winner?.EntrantId;
            }
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var logger = service.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred whilst migrating raffle entry tickets");
        }
    }
    
    public static async void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider;
        try
        {
            var context = service.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = service.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during migration");
        }
    }
}