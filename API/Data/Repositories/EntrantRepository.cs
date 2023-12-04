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

        query = entrantParams.OrderBy switch
        {
            "totalDonations" => query.OrderByDescending(entrant => entrant.TotalDonations),
            "gamertag" => query.OrderBy(entrant => entrant.NormalizedGamertag),
            _ => query.OrderBy(entrant => entrant.Id)
        };

        return await PagedList<Entrant>.CreateAsync(query, entrantParams.PageNumber, entrantParams.PageSize);
    }
}