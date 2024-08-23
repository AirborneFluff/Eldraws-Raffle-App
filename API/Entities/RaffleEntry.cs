using System.ComponentModel.DataAnnotations.Schema;
using RaffleApi.Helpers;

namespace RaffleApi.Entities;

public class RaffleEntry
{
    public int Id { get; set; }
    
    public int RaffleId { get; set; }
    public Raffle? Raffle { get; set; }
    
    public int EntrantId { get; set; }
    public Entrant? Entrant { get; set; }

    public int Donation { get; set; }
    public DateTime InputDate { get; set; } = DateTime.UtcNow;

    public int LowTicket { get; set; }
    public int HighTicket { get; set; }

    public bool Complimentary { get; set; } = false;

    [Column("Tickets")]
    private string? _tickets
    {
        get => null;
        set
        {
            if (value is null) return;
            var split = value.Split(" - ");
            LowTicket = int.Parse(split[0]);
            HighTicket = int.Parse(split[1]);
        }
    }

    [NotMapped]
    public Tuple<int, int> Tickets => new(LowTicket, HighTicket);
    
    public override string ToString()
    {
        var gamertag = Entrant?.Gamertag ?? "Unknown";
        var tickets = $"{Tickets.Item1} - {Tickets.Item2}:".PadString(200, 220);
        return tickets + gamertag;
    }
}