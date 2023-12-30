using System.Text;
using RaffleApi.Extensions;
using RaffleApi.Helpers;

namespace RaffleApi.Entities;

public class RafflePrize
{
    public int RaffleId { get; set; }
    public Raffle? Raffle { get; set; }

    public int? WinnerId { get; set; }
    public Entrant? Winner { get; set; }

    public int Place { get; set; }
    public float DonationPercentage { get; set; }
    public string? Description { get; set; }
    public int? WinningTicketNumber { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        var position = $"**{Place.AddPositionalSynonym()}**";
        sb.Append(position.PadString(65, 75));

        if (DonationPercentage == 0) return sb.Append(Description).ToString();
        
        var percentage = (int)(DonationPercentage * 100);
        sb.Append($"{percentage}% of donations");
        return sb.ToString();
    }
}