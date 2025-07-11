using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaffleApi.ActionFilters;
using RaffleApi.Data;
using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using RaffleApi.Extensions;
using RaffleApi.Helpers;
using RaffleApi.Services;

namespace RaffleApi.Controllers;

[ApiController]
[Route("api/clans/{clanId:int}/raffles")]
[ServiceFilter(typeof(ValidateClanMember))]
[Authorize]
public partial class RaffleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;
    private readonly DiscordService _discord;

    public RaffleController(IMapper mapper, UnitOfWork unitOfWork, DiscordService discord)
    {
        _unitOfWork = unitOfWork;
        _discord = discord;
        _mapper = mapper;
    }

    [HttpGet("{raffleId:int}")]
    [ServiceFilter(typeof(ValidateRaffleExists))]
    public async Task<ActionResult> GetRaffle(int raffleId, int clanId)
    {
        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        return Ok(_mapper.Map<RaffleDTO>(raffle));
    }

    [HttpGet("list")]
    public async Task<ActionResult> GetRafflesList([FromQuery] RafflesPageParams pageParams, int clanId)
    {
        var result = await _unitOfWork.RaffleRepository.GetByClan(pageParams, clanId);
        var raffles =  result.Select(r => _mapper.Map<RaffleDTO>(r));
        
        Response.AddPaginationHeader(result);
        return Ok(raffles);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateNewRaffle([FromBody] NewRaffleDTO raffleDto, int clanId)
    {
        var userId = User.GetUserId();

        var newRaffle = _mapper.Map<Raffle>(raffleDto);
        newRaffle.HostId = userId;
        newRaffle.ClanId = clanId;

        _unitOfWork.RaffleRepository.Add(newRaffle);

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(newRaffle));

        return BadRequest("Issue creating raffle");
    }

    [HttpDelete("{raffleId:int}")]
    [ServiceFilter(typeof(ValidateRaffleExists))]
    public async Task<ActionResult> DeleteRaffle(int raffleId, int clanId)
    {
        await _unitOfWork.RaffleRepository.Delete(raffleId);
        if (await _unitOfWork.Complete()) return Ok();

        return BadRequest();
    }

    [HttpPut("{raffleId:int}")]
    [ServiceFilter(typeof(ValidateRaffleExists))]
    public async Task<ActionResult> UpdateRaffle(int raffleId, [FromBody] NewRaffleDTO raffleDto, int clanId)
    {
        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);

        _mapper.Map(raffleDto, raffle);

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpPost("{raffleId:int}/prizes")]
    [ServiceFilter(typeof(ValidateRaffleExists))]
    public async Task<ActionResult> AddPrize(int raffleId, [FromBody] NewRafflePrizeDTO prizeDto, int clanId)
    {
        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        
        var newPrize = _mapper.Map<RafflePrize>(prizeDto);
        raffle.Prizes.Add(newPrize);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpDelete("{raffleId:int}/prizes/{prizePlace:int}")]
    [ServiceFilter(typeof(ValidateRaffleExists))]
    public async Task<ActionResult> RemovePrize(int raffleId, int clanId, int prizePlace)
    {
        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);

        var prize = raffle.Prizes.FirstOrDefault(p => p.Place == prizePlace);
        if (prize == null) return NotFound("No prize with that placement");

        raffle.Prizes.Remove(prize);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpPut("{raffleId:int}/prizes/{prizePlace:int}")]
    [ServiceFilter(typeof(ValidateRaffleExists))]
    public async Task<ActionResult> UpdatePrize(int raffleId, int clanId, int prizePlace, [FromBody] UpdateRafflePrizeDTO prizeDto)
    {
        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);

        var prize = raffle.Prizes.FirstOrDefault(p => p.Place == prizePlace);
        if (prize == null) return NotFound("No prize with that placement");

        _mapper.Map(prizeDto, prize);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpPost("{raffleId:int}/discord")]
    [ServiceFilter(typeof(ValidateRaffleExists))]
    public async Task<ActionResult> PostRaffleToDiscord(int raffleId, int clanId, int prizePlace)
    {
        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        var clan = await _unitOfWork.ClanRepository.GetById_Only(clanId);
        if (clan.DiscordChannelId == null) return BadRequest("This clan has no Discord channel registered");
        
        var result = await _discord.PostRaffle(raffle, (ulong)clan.DiscordChannelId);
        if (result.Failure) return BadRequest(result.ExceptionMessage ?? result.FailureMessage);

        return Ok(raffle.DiscordMessageId);
    }

    [HttpPost("{raffleId:int}/prizes/{prizePlace:int}/roll-winner")]
    [ServiceFilter(typeof(ValidateRaffleExists))]
    public async Task<ActionResult> RollWinner(int raffleId, int clanId, int prizePlace)
    {
        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        var clan = await _unitOfWork.ClanRepository.GetById_Only(clanId);
        if (clan.DiscordChannelId == null) return BadRequest("This clan has no Discord channel registered");
        
        var prize = raffle.Prizes.FirstOrDefault(p => p.Place == prizePlace);
        if (prize == null) return NotFound("No prize with that placement");

        var rollValue = RandomService.GetRandomInteger(raffle.TotalTickets, 1);

        var winner = await _unitOfWork.RaffleRepository.GetWinnerFromTicket(raffleId, rollValue);
        if (winner == null) throw new Exception($"There was an issue getting the winner for ticket: {rollValue}");

        var reroll = await _unitOfWork.RaffleRepository.HasEntrantWon(raffleId, winner.Id);

        prize.Winner = reroll ? null : winner;
        prize.WinningTicketNumber = reroll ? null : rollValue;

        var result = await _discord.SendRoll(raffle, (ulong)clan.DiscordChannelId, rollValue);
        if (result.Failure) return BadRequest(result.ExceptionMessage ?? result.FailureMessage);
        
        await _unitOfWork.Complete();
        
        return Ok(new RollWinnerDTO()
        {
            Winner = _mapper.Map<EntrantInfoDTO>(winner),
            Reroll = reroll,
            TicketNumber = rollValue
        });
    }
}