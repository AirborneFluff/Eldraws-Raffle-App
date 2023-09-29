using System.Collections.ObjectModel;

namespace RaffleApi.Entities;

public sealed class Clan
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Raffle>? Raffles { get; set; }
    public ICollection<ClanMember>? Members { get; set; }
    public ICollection<Entrant>? Entrants { get; set; }

    public bool HasMember(string userId)
    {
        var clanMember = Members?.FirstOrDefault(cm => cm.MemberId == userId);
        return clanMember != null;
    }
}