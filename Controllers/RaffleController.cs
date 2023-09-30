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
    public async Task<ActionResult> UpdateRaffle(int raffleId, [FromBody] NewRaffleDTO raffleDto, int clanId)
    {
        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        if (raffle == null) return NotFound("No raffle found by that Id");

        _mapper.Map(raffleDto, raffle);

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpPost("{raffleId:int}/entries")]
    public async Task<ActionResult> AddEntry(int raffleId, [FromBody] NewRaffleEntryDTO entryDto, int clanId)
    {
        var clan = HttpContext.GetClan();
        
        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        if (raffle == null) return NotFound("Raffle not found");

        var entrant = clan.Entrants.FirstOrDefault(e => e.Id == entryDto.EntrantId);
        if (entrant == null) return NotFound("No entrant found by that Id in this clan");
        
        var newEntry = _mapper.Map<RaffleEntry>(entryDto);
        raffle.Entries.Add(newEntry);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpDelete("{raffleId:int}/entries/{entryId:int}")]
    public async Task<ActionResult> RemoveEntry(int raffleId, int entryId, int clanId)
    {
        var raffle = await _unitOfWork.RaffleRepository.GetById(raffleId);
        if (raffle == null) return NotFound("Raffle not found");

        var entry = raffle.Entries.FirstOrDefault(e => e.Id == entryId);
        if (entry == null) return NotFound("No entry found by that Id");
        
        raffle.Entries.Remove(entry);
        
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }
    
}