using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class NewRafflePrizeDTO
{
    [Required]
    [Range(1, Int32.MaxValue)]
    public int Place { get; set; }
    
    public string? Description { get; set; }
    [Range(0f, 1f)]
    public float DonationPercentage { get; set; }
}