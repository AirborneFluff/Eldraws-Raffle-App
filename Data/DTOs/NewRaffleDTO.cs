using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class NewRaffleDTO
{
    [Required]
    public int ClanId { get; set; }
    [Required]
    public string Title { get; set; }
    public int EntryCost { get; set; } = 0;
    
    public DateTime? OpenDate { get; set; } = DateTime.Now;
    [Required]
    public DateTime? CloseDate { get; set; }
    [Required]
    public DateTime? DrawDate { get; set; }
}