using System.ComponentModel.DataAnnotations.Schema;

namespace RaffleApi.Entities;

public class RaffleEntry
{
    public int Id { get; set; }
    public int RaffleId { get; set; }
    public Raffle? Raffle { get; set; }
        
    public int EntrantId { get; set; }
    public Entrant? Entrant { get; set; }

    public int Donation { get; set; }

    [Column("Tickets")]
    private string? _tickets
    {
        get
        {
            return $"{Tickets?.Item1} - {Tickets?.Item2}";
        }
        set
        {
            var split = value.Split(" - ");
            Tickets = new Tuple<int, int>(Int32.Parse(split[0]), Int32.Parse(split[1]));
        }
    }
    public DateTime InputDate { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public Tuple<int, int>? Tickets { get; set; }
}