using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace RaffleApi.Entities;

public sealed class AppUser : IdentityUser
{
    public ICollection<Clan> Clans { get; set; } = new Collection<Clan>();

    public ICollection<Raffle> Raffles { get; set; } = new Collection<Raffle>();
}