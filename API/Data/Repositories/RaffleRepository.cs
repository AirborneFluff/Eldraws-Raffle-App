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

    public async Task<List<Raffle>?> GetAllRaffles()
    {
        return await _context.Raffles
            .ToListAsync();
    }
}