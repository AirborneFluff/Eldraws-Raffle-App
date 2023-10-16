using RaffleApi.Entities;

namespace RaffleApi.Data.DTOs;

public class ClanInfoDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public AppUserDTO Owner { get; set; }
}