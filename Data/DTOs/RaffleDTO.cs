using AutoMapper;
using RaffleApi.Entities;

namespace RaffleApi.Data.DTOs;

public sealed class RaffleDTO
{
    public int Id { get; set; }

    public ClanInfoDTO? Clan { get; set; }
    public MemberDTO? Host { get; set; }

    public string? Title { get; set; }
    public int EntryCost { get; set; }
    public ulong? DiscordMessageId { get; set; }
    public string? DiscordChannelId { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime OpenDate { get; set; }
    public DateTime CloseDate { get; set; }
    public DateTime DrawDate { get; set; }

    public IEnumerable<RaffleEntryInfoDTO>? Entries { get; set; }
    public IEnumerable<RafflePrize>? Prizes { get; set; }
}