namespace RaffleApi.Classes;

public class RaffleDrawParams
{
    public int Delay { get; set; } = 5;
    public bool PreventMultipleWins { get; set; } = false;

    public RaffleDrawParams(int delay, bool preventMultipleWins)
    {
        Delay = delay;
        PreventMultipleWins = preventMultipleWins;
    }

    public RaffleDrawParams(int delay)
    {
        Delay = delay;
    }
}