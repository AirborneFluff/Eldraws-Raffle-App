namespace RaffleApi.Data.v2.Repositories;

public class RaffleRepository
{
    private readonly DataContext _context;

    public RaffleRepository(DataContext context)
    {
        _context = context;
    }
}