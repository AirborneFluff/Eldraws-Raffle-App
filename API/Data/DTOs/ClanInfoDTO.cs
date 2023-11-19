namespace RaffleApi.Data.DTOs;

public class ClanInfoDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public MemberDTO? Owner { get; set; }
    public string? DiscordChannelId { get; set; }
}