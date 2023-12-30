namespace RaffleApi.Data.DTOs;

public class RafflePrizeInfoDTO
{
    public int Place { get; set; }
    public float DonationPercentage { get; set; }
    public string? Description { get; set; }
    public int? WinningTicketNumber { get; set; }
    public EntrantInfoDTO? Winner { get; set; }
}