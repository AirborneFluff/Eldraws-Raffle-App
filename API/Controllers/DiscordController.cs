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
        
        var result = await _discord.SendRaffleEmbed(raffle, (ulong)clan.DiscordChannelId);
        if (result.Failure) return BadRequest(result.ExceptionMessage ?? result.FailureMessage);

        return Ok();
    }

    // [HttpPost("{raffleId:int}/discord/roll")]
    // [ServiceFilter(typeof(ValidateRaffle))]
    // public async Task<ActionResult> RollWinners(int raffleId, int clanId, [FromQuery] DiscordRollDTO rollParams)
    // {
    //     var raffle = HttpContext.GetRaffle();
    //     var clan = HttpContext.GetClan();
    //     if (clan.DiscordChannelId == null) return BadRequest("This clan has no Discord channel registered");
    //     if (raffle.DiscordMessageId == null) return BadRequest("This raffle hasn't been posted to Discord yet");
    //
    //     if (rollParams.RollAll)
    //     {
    //         
    //     }
    //     
    //     if (await _unitOfWork.Complete()) return Ok();
    //     
    //     return BadRequest();
    // }
    
    
}