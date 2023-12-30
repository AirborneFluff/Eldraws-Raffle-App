using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class NewRaffleDTO
{
    [Required]
    public required string Title { get; set; }
    public int EntryCost { get; set; } = 0;
    public string? Description { get; set; }
    
    public DateTime OpenDate { get; set; } = DateTime.Now;
    [Required]
    public required DateTime CloseDate { get; set; }
    [Required]
    public required DateTime DrawDate { get; set; }
}