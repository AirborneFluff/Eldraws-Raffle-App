using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace RaffleApi.Entities;

public sealed class AppUser : IdentityUser
{
    public ICollection<ClanMember> Clans { get; set; } = new Collection<ClanMember>();
}