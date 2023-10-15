using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaffleApi.ActionFilters;
using RaffleApi.Data;
using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using RaffleApi.Extensions;
using RaffleApi.Services;

namespace RaffleApi.Controllers;

[ApiController]
[Route("api/clans/{clanId:int}/raffles")]
[ServiceFilter(typeof(ValidateClanMember))]
[Authorize]
public sealed class RaffleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public RaffleController(IMapper mapper, UnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    [HttpGet("{raffleId:int}")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> CreateNewRaffle(int raffleId, int clanId)
    {
        var raffle = HttpContext.GetRaffle();

        return Ok(_mapper.Map<RaffleDTO>(raffle));
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

    [HttpPut("{raffleId:int}")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> UpdateRaffle(int raffleId, [FromBody] NewRaffleDTO raffleDto, int clanId)
    {
        var raffle = HttpContext.GetRaffle();

        _mapper.Map(raffleDto, raffle);

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpPost("{raffleId:int}/entries")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> AddEntry(int raffleId, [FromBody] NewRaffleEntryDTO entryDto, int clanId)
    {
        var clan = HttpContext.GetClan();
        var raffle = HttpContext.GetRaffle();

        var entrant = clan.Entrants.FirstOrDefault(e => e.Id == entryDto.EntrantId);
        if (entrant == null) return NotFound("No entrant found by that Id in this clan");
        
        var newEntry = _mapper.Map<RaffleEntry>(entryDto);
        newEntry.Tickets = raffle.GetTickets(newEntry.Donation);
        
        raffle.Entries.Add(newEntry);

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpDelete("{raffleId:int}/entries/{entryId:int}")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> RemoveEntry(int raffleId, int entryId, int clanId)
    {
        var raffle = HttpContext.GetRaffle();
        
        var entry = raffle.Entries.FirstOrDefault(e => e.Id == entryId);
        if (entry == null) return NotFound("No entry found by that Id");

        raffle.Entries.Remove(entry);
        raffle.RedistributeTickets();

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpPost("{raffleId:int}/prizes")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> AddPrize(int raffleId, [FromBody] NewRafflePrizeDTO prizeDto, int clanId)
    {
        var raffle = HttpContext.GetRaffle();
        
        var newPrize = _mapper.Map<RafflePrize>(prizeDto);
        raffle.Prizes.Add(newPrize);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpDelete("{raffleId:int}/prizes/{prizePlace:int}")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> RemovePrize(int raffleId, int clanId, int prizePlace)
    {
        var raffle = HttpContext.GetRaffle();

        var prize = raffle.Prizes.FirstOrDefault(p => p.Place == prizePlace);
        if (prize == null) return NotFound("No prize with that placement");

        raffle.Prizes.Remove(prize);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpPut("{raffleId:int}/prizes/{prizePlace:int}")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> UpdatePrize(int raffleId, int clanId, int prizePlace, [FromBody] UpdateRafflePrizeDTO prizeDto)
    {
        var raffle = HttpContext.GetRaffle();

        var prize = raffle.Prizes.FirstOrDefault(p => p.Place == prizePlace);
        if (prize == null) return NotFound("No prize with that placement");

        prize.Description = prizeDto.Description;
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }
    
}