using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class NewClanDTO
{
    [Required]
    public required string Name { get; set; }

    public string? DiscordChannelId { get; set; }
}