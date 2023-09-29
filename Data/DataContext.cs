using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RaffleApi.Entities;
using RaffleApi.Helpers;

namespace RaffleApi.Data;

public sealed class DataContext : IdentityDbContext <AppUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Clan> Clans { get; set; }
    public DbSet<Raffle> Raffles { get; set; }
    public DbSet<Entrant> Entrants { get; set; }
    public DbSet<RaffleEntry> Entries { get; set; }
    public DbSet<RafflePrize> Prizes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Raffle>()
            .HasOne(r => r.AppUser)
            .WithMany(au => au.Raffles);

        modelBuilder.Entity<Entrant>()
            .HasIndex(e => e.NormalizedGamertag)
            .IsUnique();
        
        modelBuilder.Entity<Raffle>()
            .HasOne(r => r.Clan)
            .WithMany(c => c.Raffles);
    }
}