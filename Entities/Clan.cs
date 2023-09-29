using System.Collections.ObjectModel;

namespace RaffleApi.Entities;

public sealed class Clan
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Raffle> Raffles { get; set; }
    public ICollection<AppUser> Admins { get; set; } = new Collection<AppUser>();
}