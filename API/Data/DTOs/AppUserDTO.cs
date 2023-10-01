using System.Collections.ObjectModel;
using RaffleApi.Entities;

namespace RaffleApi.Data.DTOs;

public sealed class AppUserDTO
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }
}