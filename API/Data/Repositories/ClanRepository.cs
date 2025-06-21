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
            .Where(c => c.Members.FirstOrDefault(m => m.MemberId == userId) != null)
            .ToListAsync();
    }

    public Task<Clan> GetById(int id)
    {
        return _context.Clans
            .Include(c => c.Owner)
            .Include(c => c.Members)
            .ThenInclude(m => m.Member)
            .Include(c => c.Raffles)
            .Include(c => c.Entrants)
            .SingleAsync(clan => clan.Id == id);
    }

    public Task<Clan?> GetByName(string name)
    {
        return _context.Clans.FirstOrDefaultAsync(clan => clan.Name.ToUpper() == name.ToUpper());
    }

    public Task<bool> IsUserMember(int clanId, string userId)
    {
        return _context.ClanMembers
            .AnyAsync(cm => cm.MemberId == userId && cm.ClanId == clanId);
    }

    public Task<bool> IsUserOwner(int clanId, string userId)
    {
        return _context.Clans
            .AnyAsync(c => c.OwnerId == userId && c.Id == clanId);
    }

    public Task<Clan> GetById_Only(int id)
    {
        return _context.Clans
            .SingleAsync(clan => clan.Id == id);
    }

    public Task<bool> EntrantExists(int clanId, string gamertag)
    {
        return _context.Entrants
            .AnyAsync(en => en.ClanId == clanId && en.Gamertag.ToLower() == gamertag.ToLower());
    }
}