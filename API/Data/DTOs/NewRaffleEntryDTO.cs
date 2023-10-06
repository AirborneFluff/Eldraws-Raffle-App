using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class NewRaffleEntryDTO
{
    public int EntrantId { get; set; }
    [Range(0, int.MaxValue)]
    public int Donation { get; set; }
}