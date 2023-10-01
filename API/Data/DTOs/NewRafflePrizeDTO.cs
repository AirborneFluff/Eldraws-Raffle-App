using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class NewRafflePrizeDTO
{
    public int Place { get; set; }
    [Required]
    public string Description { get; set; }
}