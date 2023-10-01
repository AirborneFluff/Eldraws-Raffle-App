using Microsoft.EntityFrameworkCore;
using RaffleApi.Entities;

namespace RaffleApi.Data.Repositories;

public sealed class ClanRepository
{
    private readonly DataContext _context;

    public ClanRepository(DataContext context) {
        _context = context;
    }

    public void Add(Clan clan)
    {
        _context.Clans.Add(clan);
    }

    public void Remove(Clan clan)
    {
        _context.Clans.Remove(clan);
    }

    public Task<List<Clan>> GetAllForUser(string userId)
    {
        return _context.Clans
            .Include(c => c.Owner)
            .Include(c => c.Members)
            .Include(c => c.Entrants)
            .Include(c => c.Raffles)
            .Where(c => c.Members.FirstOrDefault(m => m.MemberId == userId) != null)
            .ToListAsync();
    }

    public Task<Clan?> GetById(int id)
    {
        return _context.Clans
            .Include(c => c.Owner)
            .Include(c => c.Members)
            .Include(c => c.Entrants)
            .Include(c => c.Raffles)
            .FirstOrDefaultAsync(clan => clan.Id == id);
    }

    public Task<Clan?> GetByName(string name)
    {
        return _context.Clans.FirstOrDefaultAsync(clan => clan.Name.ToUpper() == name.ToUpper());
    }
}