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
            .Include(r => r.AppUser)
            .Include(r => r.Prizes.OrderBy(p => p.Place))
            .Include(r => r.Entries)
            .ThenInclude(e => e.Entrant)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Raffle>?> GetAllRaffles()
    {
        return await _context.Raffles
            .Include(r => r.AppUser)
            .ToListAsync();
    }

    public async Task<List<Entrant>> GetAllEntrants()
    {
        return await _context.Entrants.ToListAsync();
    }
    public async Task<Entrant?> GetEntrantById(int id)
    {
        return await _context.Entrants.FirstOrDefaultAsync(e => e.Id == id);
    }
    public async Task<Entrant?> GetEntrantByGamertag(string gamertag)
    {
        return await _context.Entrants.FirstOrDefaultAsync(e => e.NormalizedGamertag == gamertag.ToUpper());
    }
    public void AddNewEntrant(Entrant entrant)
    {
        _context.Entrants.Add(entrant);
    }
}