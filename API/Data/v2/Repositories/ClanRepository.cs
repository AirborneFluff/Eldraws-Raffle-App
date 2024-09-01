using Microsoft.EntityFrameworkCore;
using RaffleApi.Entities;

namespace RaffleApi.Data.v2.Repositories;

public class ClanRepository
{
    private readonly DataContext _context;

    public ClanRepository(DataContext context)
    {
        _context = context;
    }

    public Task<bool> Exists(string clanName)
    {
        return _context.Clans
            .AnyAsync(clan => clan.NormalizedName == clanName.ToUpper());
    }
    
    public void Add(Clan clan)
    {
        _context.Clans.Add(clan);
    }

    public Task<Clan> GetById(int id)
    {
        return _context.Clans.SingleAsync(clan => clan.Id == id);
    }
}