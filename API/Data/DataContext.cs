using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RaffleApi.Entities;
using RaffleApi.Helpers;

namespace RaffleApi.Data;

public sealed class DataContext : IdentityDbContext <AppUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) {}

    public DbSet<Clan> Clans { get; set; }
    public DbSet<ClanMember> ClanMembers { get; set; }
    public DbSet<Raffle> Raffles { get; set; }
    public DbSet<Entrant> Entrants { get; set; }
    public DbSet<RaffleEntry> Entries { get; set; }
    public DbSet<RafflePrize> Prizes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // ---------- Clan Members ----------
        modelBuilder.Entity<ClanMember>()
            .HasOne(cm => cm.Clan)
            .WithMany(c => c.Members)
            .HasForeignKey(cm => cm.ClanId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ClanMember>()
            .HasOne(cm => cm.Member)
            .WithMany(m => m.Clans)
            .HasForeignKey(cm => cm.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // ---------- Clan ----------
        modelBuilder.Entity<Clan>()
            .HasMany(c => c.Raffles)
            .WithOne(r => r.Clan)
            .HasForeignKey(r => r.ClanId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Clan>()
            .HasMany(c => c.Entrants)
            .WithOne(e => e.Clan)
            .HasForeignKey(e => e.ClanId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Clan>()
            .HasOne(c => c.Owner)
            .WithMany(o => o.OwnedClans)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Clan>()
            .HasIndex(c => c.NormalizedName)
            .IsUnique();
        
        // ---------- Raffle ----------
        modelBuilder.Entity<Raffle>()
            .HasOne(r => r.Host)
            .WithMany(h => h.HostedRaffles)
            .HasForeignKey(r => r.HostId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Raffle>()
            .Property(ExpressionHelper.GetMember<Raffle, string>("_additionalMessageIds"));
        
        // ---------- Raffle Entry ----------
        modelBuilder.Entity<RaffleEntry>()
            .Property(ExpressionHelper.GetMember<RaffleEntry, string>("_tickets"));
        
        modelBuilder.Entity<RaffleEntry>()
            .HasOne(re => re.Raffle)
            .WithMany(r => r.Entries)
            .HasForeignKey(re => re.RaffleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<RaffleEntry>()
            .HasOne(re => re.Entrant)
            .WithMany(e => e.Entries)
            .HasForeignKey(re => re.EntrantId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // ---------- Raffle Prizes ----------
        modelBuilder.Entity<RafflePrize>()
            .HasKey(rp => new { rp.Place, rp.RaffleId });
        
        modelBuilder.Entity<RafflePrize>()
            .HasOne(rp => rp.Raffle)
            .WithMany(r => r.Prizes)
            .HasForeignKey(rp => rp.RaffleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RafflePrize>()
            .HasOne(rp => rp.Winner)
            .WithMany(e => e.Prizes)
            .OnDelete(DeleteBehavior.NoAction);
        
        // ---------- Raffle Prizes ----------
        modelBuilder.Entity<Entrant>()
            .HasIndex(e => new { e.ClanId, e.NormalizedGamertag })
            .IsUnique();

        modelBuilder.Entity<Entrant>()
            .HasIndex(e => e.ClanId);

        modelBuilder.Entity<Entrant>()
            .HasIndex(e => e.NormalizedGamertag);

        modelBuilder.Entity<Entrant>()
            .HasIndex(e => new { e.Active, e.TotalDonations });

        modelBuilder.Entity<Entrant>()
            .HasIndex(e => new { e.Active, e.NormalizedGamertag });
    }
}