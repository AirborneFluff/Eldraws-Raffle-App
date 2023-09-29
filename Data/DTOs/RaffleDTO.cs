using RaffleApi.Entities;

namespace RaffleApi.Data.DTOs;

public sealed class RaffleDTO
{
    public int Id { get; set; }
    public string AppUserId { get; set; }

    public string Title { get; set; }
    public int EntryCost { get; set; }
    public ulong? DiscordMessageId { get; set; }
    public string? DiscordChannelId { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime CloseDate { get; set; }
    public DateTime DrawDate { get; set; }

    public IEnumerable<RaffleEntryDTO>? Entries { get; set; }
    public IEnumerable<RafflePrize>? Prizes { get; set; }
}