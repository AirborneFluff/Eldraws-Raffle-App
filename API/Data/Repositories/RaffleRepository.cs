using Microsoft.EntityFrameworkCore;
using RaffleApi.Entities;
using RaffleApi.Extensions;

namespace RaffleApi.Data.Repositories;

public class RaffleRepository
{
    
    private readonly DataContext _context;
    public RaffleRepository(DataContext context)
    {
        this._context = context;
    }

    public void Add(Raffle raffle)
    {
        _context.Raffles.Add(raffle);
    }
    
    public void Delete(Raffle raffle)
    {
        _context.Raffles.Remove(raffle);
    }
    
    public async Task Delete(int raffleId)
    {
        var raffle = await _context.Raffles.FirstOrDefaultAsync(r => r.Id == raffleId);
        if (raffle == null) return;
        _context.Raffles.Remove(raffle);
    }

    public Task<bool> Exists(int id)
    {
        return _context.Raffles
            .Where(r => r.Id == id)
            .AnyAsync();
    }

    public async Task<Raffle> GetById(int id)
    {
        return await _context.Raffles
            .Include(r => r.Clan)
            .ThenInclude(c => c!.Members)
            .Include(r => r.Host)
            .Include(r => r.Prizes.OrderBy(p => p.Place))
            .ThenInclude(p => p.Winner)
            .SingleAsync(r => r.Id == id);
    }

    public async Task<int> GetNextAvailableTicket(int raffleId)
    {
        if (await _context.Entries
            .Where(entry => entry.RaffleId == raffleId)
            .AnyAsync() == false) return 1;
        
        return await _context.Entries
            .Where(entry => entry.RaffleId == raffleId)
            .MaxAsync(entry => entry.HighTicket + 1);

    }

    public async Task<Entrant?> GetWinnerFromTicket(int raffleId, int ticketNumber)
    {
        var entry = await _context.Entries
            .Where(entry => entry.RaffleId == raffleId)
            .FirstOrDefaultAsync(entry => ticketNumber >= entry.LowTicket && ticketNumber <= entry.HighTicket);
        
        if (entry is null) return null;
        return await _context.Entrants.SingleAsync(entrant => entrant.Id == entry.EntrantId);
    }

    public async Task<bool> HasAnyWinners(int raffleId)
    {
        return await _context.Prizes
            .Where(prize => prize.RaffleId == raffleId)
            .AnyAsync(prize => prize.WinningTicketNumber != null);
    }

    public async Task<bool> HasEntrantWon(int raffleId, int entrantId)
    {
        var prize = await _context.Prizes
            .Where(prize => prize.RaffleId == raffleId)
            .FirstOrDefaultAsync(prize => prize.WinnerId == entrantId);
        
        return prize is not null;
    }

    public async Task RedistributeTickets(int raffleId)
    {
        var raffle = await _context.Raffles.FirstOrDefaultAsync(raffle => raffle.Id == raffleId);
        if (raffle is null) return;
        
        var query = _context.Entries
            .Where(entry => entry.RaffleId == raffleId)
            .OrderBy(entry => entry.LowTicket)
            .AsEnumerable();
        
        var lowTicket = 1;
        
        foreach (var entry in query)
        {
            var highTicket = raffle.GetHighTicket(entry.Donation, lowTicket);
            if (highTicket == 0) continue;
        
            entry.LowTicket = lowTicket;
            entry.HighTicket = highTicket;
            
            lowTicket = highTicket + 1;
        }
    }
}