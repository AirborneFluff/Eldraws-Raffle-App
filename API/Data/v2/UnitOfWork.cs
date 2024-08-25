using RaffleApi.Data.v2.Repositories;

namespace RaffleApi.Data.v2;

public sealed class UnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }
    
    public ClanRepository ClanRepository => new (_context);
    public RaffleRepository RaffleRepository => new (_context);

    public async Task<bool> Complete()
    {
        try { return await _context.SaveChangesAsync() > 0; }
        catch { return false; }
    }
}