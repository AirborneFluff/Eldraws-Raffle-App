using System.Collections.ObjectModel;

namespace RaffleApi.Entities;

public sealed class Clan
{
    public int Id { get; set; }
    public string Name { get; set; }

    public AppUser? Owner { get; set; }
    public string OwnerId { get; set; }

    public string NormalizedName { get => Name.ToUpper(); set => Name.ToUpper(); }

    public ICollection<Raffle> Raffles { get; set; } = new Collection<Raffle>();
    public ICollection<ClanMember> Members { get; set; } = new Collection<ClanMember>();
    public ICollection<Entrant> Entrants { get; set; } = new Collection<Entrant>();

    public bool HasMember(string userId)
    {
        var clanMember = Members?.FirstOrDefault(cm => cm.MemberId == userId);
        return clanMember != null;
    }
}