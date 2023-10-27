namespace RaffleApi.Entities;

public class RafflePrize
{
    public int RaffleId { get; set; }
    public Raffle? Raffle { get; set; }

    public int Place { get; set; }
    public float DonationPercentage { get; set; }
    public string? Description { get; set; }
    public int? WinningTicketNumber { get; set; }
}