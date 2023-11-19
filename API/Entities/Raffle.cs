using System.Collections.ObjectModel;

namespace RaffleApi.Entities;

public class Raffle
{
    public int Id { get; set; }

    public int ClanId { get; set; }
    public Clan? Clan { get; set; }

    public required string HostId { get; set; }
    public AppUser? Host { get; set; }

    public required string Title { get; set; }
    public int EntryCost { get; set; }
    public ulong? DiscordMessageId { get; set; }
    public string? DiscordChannelId { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime OpenDate { get; set; }
    public DateTime CloseDate { get; set; }
    public DateTime DrawDate { get; set; }

    public ICollection<RaffleEntry> Entries { get; set; } = new Collection<RaffleEntry>();
    public ICollection<RafflePrize> Prizes { get; set; } = new Collection<RafflePrize>();

    public bool HasMember(string userId)
    {
        if (Clan == null) throw new Exception("Clan not included for evaluation");
        return Clan.HasMember(userId);
    } 
}