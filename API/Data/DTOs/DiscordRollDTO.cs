namespace RaffleApi.Data.DTOs;

public class DiscordRollDTO
{
    public bool RollAll { get; set; } = true;
    public float DelayBetween { get; set; } = 10;
}