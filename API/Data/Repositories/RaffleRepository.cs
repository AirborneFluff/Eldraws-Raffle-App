using Microsoft.EntityFrameworkCore;
using RaffleApi.Entities;

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

    public async Task<Raffle?> GetById(int id)
    {
        return await _context.Raffles
            .Include(r => r.Clan)
            .ThenInclude(c => c!.Members)
            .Include(r => r.Host)
            .Include(r => r.Prizes.OrderBy(p => p.Place))
            .ThenInclude(p => p.Winner)
            .Include(r => r.Entries)
            .ThenInclude(e => e.Entrant)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<int> GetNextAvailableTicket(int raffleId)
    {
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

    public async Task<bool> HasEntrantWon(int raffleId, int entrantId)
    {
        var prize = await _context.Prizes
            .FirstOrDefaultAsync(prize => prize.WinnerId == entrantId);
        
        return prize is not null;
    }
}