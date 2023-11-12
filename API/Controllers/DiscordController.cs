using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaffleApi.ActionFilters;
using RaffleApi.Classes;
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

    public DiscordController(DiscordService discord)
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
        
        var result = await _discord.PostRaffle(raffle, (ulong)clan.DiscordChannelId);
        if (result.Failure) return BadRequest(result.ExceptionMessage ?? result.FailureMessage);

        return Ok(raffle.DiscordMessageId);
    }

    [HttpPost("{raffleId:int}/discord/roll")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> RollWinners(int raffleId, int clanId, [FromQuery] RaffleDrawParams options)
    {
        var raffle = HttpContext.GetRaffle();
        var clan = HttpContext.GetClan();
        if (clan.DiscordChannelId == null) return BadRequest("This clan has no Discord channel registered");
        if (raffle.DiscordMessageId == null) return BadRequest("This raffle hasn't been posted to Discord yet");
        
        var result = await _discord.RollWinners(raffle, (ulong)clan.DiscordChannelId, options);
        if (result.Failure) return BadRequest(result.ExceptionMessage ?? result.FailureMessage);

        return Ok();
    }
    
    
}