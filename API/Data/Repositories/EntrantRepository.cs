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
    
    public async Task<PagedList<Entrant>> GetByClan(EntrantParams entrantParams, int clanId)
    {
        var query = _context.Entrants
            .AsNoTracking()
            .Where(e => e.ClanId == clanId);

        var searchTerm = entrantParams.Gamertag?.ToUpper();
        if (searchTerm is not null)
        {
            query = query.Where(e => e.NormalizedGamertag.Contains(searchTerm));
        }

        query = entrantParams.OrderBy switch
        {
            "totalDonations" => query
                .OrderByDescending(entrant => entrant.Active)
                .ThenByDescending(entrant => entrant.TotalDonations),
            _ => query
                .OrderByDescending(entrant => entrant.Active)
                .ThenBy(entrant => entrant.NormalizedGamertag)
        };
        

        return await PagedList<Entrant>.CreateAsync(query, entrantParams.PageNumber, entrantParams.PageSize);
    }
    
    public Task<List<Entrant>> SearchByGamertag(string gamertag, int clanId)
    {
        return _context.Entrants
            .AsNoTracking()
            .Where(e => e.ClanId == clanId)
            .Where(e => e.NormalizedGamertag.Contains(gamertag.ToUpper()))
            .OrderBy(e => e.NormalizedGamertag)
            .Take(5)
            .ToListAsync();
    }

    public Task<Entrant?> GetById(int entrantId)
    {
        return _context.Entrants.FirstOrDefaultAsync(entrant => entrant.Id == entrantId);
    }
}