using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaffleApi.ActionFilters;
using RaffleApi.Data;
using RaffleApi.Extensions;
using RaffleApi.Services;

namespace RaffleApi.Controllers;

[ApiController]
[Route("api/clans/{clanId:int}/raffles")]
[ServiceFilter(typeof(ValidateClanMember))]
[Authorize]
public class DiscordController : ControllerBase
{
    private readonly DiscordService _discord;

    public DiscordController(IMapper mapper, UnitOfWork unitOfWork, DiscordService discord)
    {
        _discord = discord;
    }

    [HttpPost("{raffleId:int}/discord")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> PostDiscordEmbed(int raffleId, int clanId)
    {
        var raffle = HttpContext.GetRaffle();
        var clan = HttpContext.GetClan();
        if (clan.DiscordChannelId == null) return BadRequest("This clan has no Discord channel registered");
        await _discord.SendRaffleEmbed(raffle, (ulong)clan.DiscordChannelId);
        
        return Ok();
    }
    
    
}