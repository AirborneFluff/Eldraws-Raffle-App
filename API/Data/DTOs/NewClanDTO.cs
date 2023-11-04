using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class NewClanDTO
{
    [Required]
    public string Name { get; set; }

    public string? DiscordChannelId { get; set; }
}