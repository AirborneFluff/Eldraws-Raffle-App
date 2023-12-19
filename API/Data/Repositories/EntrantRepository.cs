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
            .Where(e => e.ClanId == clanId);

        var searchTerm = entrantParams.Gamertag?.ToUpper();
        if (searchTerm is not null)
        {
            query = query.Where(e => e.NormalizedGamertag.Contains(searchTerm));
        }

        query = entrantParams.OrderBy switch
        {
            "totalDonations" => query.OrderByDescending(entrant => entrant.TotalDonations),
            _ => query.OrderBy(entrant => entrant.NormalizedGamertag)
        };

        return await PagedList<Entrant>.CreateAsync(query, entrantParams.PageNumber, entrantParams.PageSize);
    }
}