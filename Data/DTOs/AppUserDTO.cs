using System.Collections.ObjectModel;
using RaffleApi.Entities;

namespace RaffleApi.Data.DTOs;

public sealed class AppUserDTO
{
    public string Id { get; set; }
    public string? Token { get; set; }
    
    public ICollection<ClanDTO> UserClans { get; set; } = new Collection<ClanDTO>();
}