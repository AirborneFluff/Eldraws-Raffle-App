using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Classes;

public class RaffleDrawParams
{
    [Range(0, 30)]
    public int Delay { get; set; } = 5;
    [Range(1, 50)]
    public int MaxRerolls { get; set; } = 10;
    public bool PreventMultipleWins { get; set; } = true;
}