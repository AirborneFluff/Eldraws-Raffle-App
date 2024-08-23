using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class NewRaffleEntryDTO
{
    [Required]
    public int EntrantId { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int Donation { get; set; }

    public bool Complimentary { get; set; } = false;
}