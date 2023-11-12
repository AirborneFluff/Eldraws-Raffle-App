namespace RaffleApi.Classes;

public class RaffleDrawParams
{
    public int Delay { get; set; }
    public bool PreventMultipleWins { get; set; }

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