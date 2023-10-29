using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class UpdateRafflePrizeDTO
{
    public string? Description { get; set; }
    [Range(0f, 1f)]
    public float DonationPercentage { get; set; }
}