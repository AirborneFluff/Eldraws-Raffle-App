using System.Collections.ObjectModel;

namespace RaffleApi.Entities;

public class Entrant
{
    public int Id { get; set; }
    public int ClanId { get; set; }
    public required string Gamertag { get; set; }
    public int TotalDonations { get; set; }

    public string NormalizedGamertag
    {
        get => Gamertag.ToUpper();
        set {}
    }
    
    public Clan? Clan { get; set; }
    public ICollection<RaffleEntry> Entries { get; set; } = new Collection<RaffleEntry>();
}