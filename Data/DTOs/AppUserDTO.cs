using System.Collections.ObjectModel;
using RaffleApi.Entities;

namespace RaffleApi.Data.DTOs;

public sealed class AppUserDTO
{
    public Guid Id { get; set; }
    public string? Token { get; set; }
    
    public ICollection<Clan> UserClans { get; set; } = new Collection<Clan>();
}