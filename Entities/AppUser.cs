using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace RaffleApi.Entities;

public sealed class AppUser : IdentityUser
{
    public ICollection<Clan> UserClans { get; set; } = new Collection<Clan>();

    public ICollection<Raffle> Raffles { get; set; } = new Collection<Raffle>();
}