using System.Collections.ObjectModel;

namespace RaffleApi.Entities;

public class Entrant
{
    public int Id { get; set; }
    public int ClanId { get; set; }
    public required string Gamertag { get; set; }
    public int TotalDonations { get; set; }

    public bool Active { get; set; } = true;

    public string NormalizedGamertag
    {
        get => Gamertag.ToUpper();
        set {}
    }
    
    public Clan? Clan { get; set; }
    public ICollection<RaffleEntry> Entries { get; set; } = new Collection<RaffleEntry>();
    public ICollection<RafflePrize> Prizes { get; set; } = new Collection<RafflePrize>();
}