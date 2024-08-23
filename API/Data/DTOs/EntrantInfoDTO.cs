namespace RaffleApi.Data.DTOs;

public class EntrantInfoDTO
{
    public int Id { get; set; }
    public required string Gamertag { get; set; }
    public bool Active { get; set; }
}