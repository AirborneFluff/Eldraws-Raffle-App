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
    private readonly UnitOfWork _unitOfWork;
    private readonly DiscordService _discord;

    public DiscordController(IMapper mapper, UnitOfWork unitOfWork, DiscordService discord)
    {
        _unitOfWork = unitOfWork;
        _discord = discord;
    }

    [HttpPost("{raffleId:int}/discord")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> PostDiscordEmbed(int raffleId, int clanId)
    {
        var raffle = HttpContext.GetRaffle();
        var clan = HttpContext.GetClan();
        if (clan.DiscordChannelId == null) return BadRequest("This clan has no Discord channel registered");
        var messageId = await _discord.SendRaffleEmbed(raffle, (ulong)clan.DiscordChannelId);

        if (messageId == null) return BadRequest("Couldn't send message to Discord");

        if (raffle.DiscordMessageId == messageId) return Ok();
        
        raffle.DiscordMessageId = messageId;
        if (await _unitOfWork.Complete()) return Ok();
        
        return BadRequest();
    }
    
    
}