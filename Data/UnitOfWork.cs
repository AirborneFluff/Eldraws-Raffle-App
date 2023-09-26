using AutoMapper;
using RaffleApi.Data.Repositories;

namespace RaffleApi.Data;

public sealed class UnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    // Implement an interface and setup a repository like this
    public ClanRepository ClanRepository => new ClanRepository(_context);

    public async Task<bool> Complete()
    {
        try { return await _context.SaveChangesAsync() > 0; }
        catch { return false; }
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}