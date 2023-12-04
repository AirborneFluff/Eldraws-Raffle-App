namespace RaffleApi.Data.DTOs;

public class EntrantDTO
{
    public int Id { get; set; }
    public required string Gamertag { get; set; }
    public int TotalDonations { get; set; }
}