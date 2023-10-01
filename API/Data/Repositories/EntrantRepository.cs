using Microsoft.EntityFrameworkCore;
using RaffleApi.Entities;

namespace RaffleApi.Data.Repositories;

public sealed class EntrantRepository
{
    private readonly DataContext _context;

    public EntrantRepository(DataContext context) {
        _context = context;
    }
    
    public async Task<List<Entrant>> GetAllByClan(int clanId)
    {
        return await _context.Entrants
            .Where(e => e.ClanId == clanId)
            .ToListAsync();
    }
    public async Task<Entrant?> GetByGamertag(string gamertag)
    {
        return await _context.Entrants.FirstOrDefaultAsync(e => e.NormalizedGamertag == gamertag.ToUpper());
    }

    public void Add(Entrant entrant)
    {
        _context.Entrants.Add(entrant);
    }
}