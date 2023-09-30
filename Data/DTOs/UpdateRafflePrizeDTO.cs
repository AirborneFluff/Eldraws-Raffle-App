using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class UpdateRafflePrizeDTO
{
    [Required]
    public string Description { get; set; }
}