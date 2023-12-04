using Microsoft.EntityFrameworkCore;
using RaffleApi.Entities;
using RaffleApi.Helpers;

namespace RaffleApi.Data.Repositories;

public sealed class EntrantRepository
{
    private readonly DataContext _context;

    public EntrantRepository(DataContext context) {
        _context = context;
    }
    
    public async Task<PagedList<Entrant>> GetByClan(PaginationParams pagination, int clanId)
    {
        var query = _context.Entrants
            .Where(e => e.ClanId == clanId)
            .OrderBy(e => e.Id);

        return await PagedList<Entrant>.CreateAsync(query, pagination.PageNumber, pagination.PageSize);
    }
}