using Microsoft.AspNetCore.Mvc;
using RaffleApi.ActionFilters;
using RaffleApi.Data.DTOs;
using RaffleApi.Entities;
using RaffleApi.Extensions;
using RaffleApi.Helpers;

namespace RaffleApi.Controllers;

public partial class RaffleController
{
    [HttpPost("{raffleId:int}/entries")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> AddEntry(int raffleId, [FromBody] NewRaffleEntryDTO entryDto, int clanId)
    {
        var clan = HttpContext.GetClan();
        var raffle = HttpContext.GetRaffle();

        var entrant = clan.Entrants.FirstOrDefault(e => e.Id == entryDto.EntrantId);
        if (entrant == null) return NotFound("No entrant found by that Id in this clan");
        
        var newEntry = _mapper.Map<RaffleEntry>(entryDto);
        var nextTicket = await _unitOfWork.RaffleRepository.GetNextAvailableTicket(raffleId);
        
        var highTicket = raffle.GetHighTicket(newEntry.Donation, nextTicket);
        
        newEntry.LowTicket = highTicket == 0 ? 0 : nextTicket;
        newEntry.HighTicket = highTicket;

        raffle.Entries.Add(newEntry);
        
        entrant.TotalDonations += newEntry.Donation;
        raffle.TotalTickets += newEntry.HighTicket == 0 ? 0 : newEntry.HighTicket - newEntry.LowTicket + 1;
        raffle.TotalDonations += newEntry.Donation;

        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }

    [HttpDelete("{raffleId:int}/entries/{entryId:int}")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> RemoveEntry(int raffleId, int entryId, int clanId)
    {
        var clan = HttpContext.GetClan();
        var raffle = HttpContext.GetRaffle();

        var entry = await _unitOfWork.EntryRepository.GetById(entryId);
        if (entry == null) return NotFound("No entry found by that Id");
        
        var entrant = clan.Entrants.FirstOrDefault(e => e.Id == entry.EntrantId);
        if (entrant == null) return BadRequest("Issue finding entrant to update");

        raffle.Entries.Remove(entry);
        
        entrant.TotalDonations -= entry.Donation;
        raffle.TotalTickets -= entry.HighTicket == 0 ? 0 : entry.HighTicket - entry.LowTicket + 1;
        raffle.TotalDonations -= entry.Donation;
        
        if (!await _unitOfWork.Complete()) return BadRequest("Issue updating database");

        await _unitOfWork.RaffleRepository.RedistributeTickets(raffle.Id);
        if (await _unitOfWork.Complete()) return Ok(_mapper.Map<RaffleDTO>(raffle));

        return BadRequest();
    }
    
    [HttpGet("{raffleId:int}/entries")]
    [ServiceFilter(typeof(ValidateRaffle))]
    public async Task<ActionResult> GetRaffleEntries(int clanId, int raffleId, [FromQuery]RaffleEntryParams pageParams)
    {
        var result = await _unitOfWork.EntryRepository.GetByRaffle(raffleId, pageParams);
        var entries =  result.Select(entry => _mapper.Map<RaffleEntryDTO>(entry));
        
        Response.AddPaginationHeader(result);
        return Ok(entries);
    }
}