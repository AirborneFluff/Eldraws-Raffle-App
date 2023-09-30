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
    public DateTime InputDate { get; set; } = DateTime.UtcNow;
}