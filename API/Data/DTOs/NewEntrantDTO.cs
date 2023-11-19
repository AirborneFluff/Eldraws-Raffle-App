using System.ComponentModel.DataAnnotations;

namespace RaffleApi.Data.DTOs;

public class NewEntrantDTO
{
    [Required]
    public required string Gamertag { get; set; }
}