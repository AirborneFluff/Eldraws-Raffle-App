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

    public Task<Clan?> GetById(int id)
    {
        return _context.Clans.FirstOrDefaultAsync(clan => clan.Id == id);
    }

    public Task<Clan?> GetByName(string name)
    {
        return _context.Clans.FirstOrDefaultAsync(clan => clan.Name.ToUpper() == name.ToUpper());
    }
}