using Microsoft.EntityFrameworkCore;
using RaffleApi.Entities;
using RaffleApi.Helpers;

namespace RaffleApi.Data.Repositories;

public sealed class RaffleEntryRepository
{
    private readonly DataContext _context;

    public RaffleEntryRepository(DataContext _context)
    {
        this._context = _context;
    }

    public async Task<RaffleEntry?> GetById(int entryId)
    {
        return await _context.Entries.FirstOrDefaultAsync(entry => entry.Id == entryId);
    }

    public async Task<PagedList<RaffleEntry>> GetByRaffle(int raffleId, RaffleEntryParams entryParams)
    {
        var query = _context.Entries
            .Include(entry => entry.Entrant)
            .Where(entry => entry.RaffleId == raffleId)
            .OrderByDescending(entry => entry.HighTicket);

        return await PagedList<RaffleEntry>.CreateAsync(query, entryParams.PageNumber, entryParams.PageSize);
    }
}